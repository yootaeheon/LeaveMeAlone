using Firebase.Database;
using System;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Extensions;
using Inventory.Model;
using GoogleMobileAds.Api;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    public UserGameDataDTO GameData;

    public DatabaseReference userDataRef { get; private set; }

    [SerializeField] CharacterModel _model;
    public CharacterModel Model => _model ??= FindAnyObjectByType<CharacterModel>();

    [SerializeField] ProgressSO _progressData;
    public ProgressSO ProgressData => _progressData ??= Resources.Load<ProgressSO>("ProgressData");

    [SerializeField] UI_Progress _progressUI;

    [SerializeField] InventorySO _inventoryData;
    public InventorySO InventoryData => _inventoryData ??= Resources.Load<InventorySO>("InventoryData");

    public bool IsGameDataLoaded { get; private set; }
    public Action OnGameDataLoaded { get; set; }

    private void Awake()
    {
        SetSingleton();

        // ������ ��忡�� ���� ����
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
    }

    private void Start()
    {
        if (BackendManager.Instance.OnFirebaseReady)
        {
            LoadAllGameData();
        }
    }

    #region �̱��� ����
    public void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveAllGameData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveAllGameData();
    }

#if UNITY_EDITOR
    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            SaveAllGameData();
            Debug.Log("������ ���� ���� �� SaveAllGameData ȣ��");
        }
    }
#endif

    public void SaveAllGameData()
    {
        string userId = BackendManager.Auth?.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(userId)) return;

        if (Model == null || ProgressData == null || InventoryData == null)
        {
            Debug.LogWarning("SaveAllGameData ����: ���� Null��.");
            return;
        }

        userDataRef = BackendManager.Database.RootReference.Child(userId);

        CharacterModelDTO characterDTO = new CharacterModelDTO
        (
            Model.MaxHp,
            Model.RecoverHpPerSecond,
            Model.DefensePower,
            Model.AttackPower,
            Model.AttackSpeed,
            Model.CriticalChance
        );

        ProgressDataDTO progressDTO = new ProgressDataDTO
        (
            ProgressData.Chapter,
            ProgressData.Stage,
            ProgressData.KillCount
        );

        InventoryDTO inventoryDataDTO = new InventoryDTO(InventoryData);

        GameData = new UserGameDataDTO(characterDTO, progressDTO, inventoryDataDTO);
        string json = JsonUtility.ToJson(GameData);

        userDataRef.Child("gameData")
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("��� ���� ������ ���� �Ϸ�!");
                else
                    Debug.LogWarning("���� ������ ���� ����: " + task.Exception);
            });
    }

    #region ��� ������ ������Ʈ
    public void LoadAllGameData()
    {
        string userId = BackendManager.Auth?.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(userId)) return;

        BackendManager.Database.RootReference.Child(userId).Child("gameData").GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    if (_model == null || _progressData == null || _inventoryData == null) return;

                    string json = task.Result.GetRawJsonValue();
                    GameData = JsonUtility.FromJson<UserGameDataDTO>(json);

                    // ĳ���� ������
                    Model.MaxHp = GameData.CharacterModelDTO.MaxHp;
                    Model.CurHp = Model.MaxHp;
                    Model.RecoverHpPerSecond = GameData.CharacterModelDTO.RecoverHpPerSecond;
                    Model.DefensePower = GameData.CharacterModelDTO.DefensePower;
                    Model.AttackPower = GameData.CharacterModelDTO.AttackPower;
                    Model.AttackSpeed = GameData.CharacterModelDTO.AttackSpeed;
                    Model.CriticalChance = GameData.CharacterModelDTO.CriticalChance;
                    Debug.Log("�� ������ �ҷ����� �Ϸ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                    // ���൵ ������
                    ProgressData.Chapter = GameData.ProgressDataDTO.Chapter;
                    ProgressData.Stage = GameData.ProgressDataDTO.Stage;
                    ProgressData.KillCount = GameData.ProgressDataDTO.KillCount;
                    _progressUI.UpdateProgressSlider();

                    //�κ��丮 ������
                    // �κ��丮 ������ ����
                    InventoryData.Init(); // ���� ���
                    for (int i = 0; i < GameData.InventoryDataDTO.Items.Count; i++)
                    {
                        var itemDTO = GameData.InventoryDataDTO.Items[i];
                        if (itemDTO.ItemID != 0) // 0�̸� �� ����
                        {
                            // ItemSO ���ҽ� �ε� (��γ� �̸� ��Ģ �ʿ�)
                            ItemSO itemSO = Resources.Load<ItemSO>($"Items/{itemDTO.ItemID}");
                            if (itemSO != null)
                            {
                                InventoryData.RemoveItem(i, InventoryData.GetItemIndex(i).Quantity); // ���� ����
                                InventoryData.AddItem(new InventoryItem
                                {
                                    Item = itemSO,
                                    Quantity = itemDTO.Quantity
                                });
                            }
                        }
                    }


                    IsGameDataLoaded = true;
                    OnGameDataLoaded?.Invoke();

                    Debug.Log("��� ���� ������ �ҷ����� �Ϸ�!");
                }
                else
                {
                    Debug.LogWarning("���� ������ �ҷ����� ����: " + task.Exception);
                }
            });
    }
    #endregion
}
