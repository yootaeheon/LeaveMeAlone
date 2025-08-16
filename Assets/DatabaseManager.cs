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

    public UserGameDataDTO LoadData;

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

    /// <summary>
    /// ������ ����
    /// </summary>
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

        LoadData = new UserGameDataDTO(characterDTO, progressDTO, inventoryDataDTO);
        string json = JsonUtility.ToJson(LoadData);

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

    /// <summary>
    /// ��� ������ �ҷ����� 
    /// </summary>
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
                    LoadData = JsonUtility.FromJson<UserGameDataDTO>(json);

                    LoadModelData();
                    LoadProgressData();
                    LoadInventoryData();

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

    public void LoadModelData()
    {
        Model.MaxHp = LoadData.CharacterModelDTO.MaxHp;
        Model.CurHp = Model.MaxHp;
        Model.RecoverHpPerSecond = LoadData.CharacterModelDTO.RecoverHpPerSecond;
        Model.DefensePower = LoadData.CharacterModelDTO.DefensePower;
        Model.AttackPower = LoadData.CharacterModelDTO.AttackPower;
        Model.AttackSpeed = LoadData.CharacterModelDTO.AttackSpeed;
        Model.CriticalChance = LoadData.CharacterModelDTO.CriticalChance;
    }

    public void LoadProgressData()
    {
        ProgressData.Chapter = LoadData.ProgressDataDTO.Chapter;
        ProgressData.Stage = LoadData.ProgressDataDTO.Stage;
        ProgressData.KillCount = LoadData.ProgressDataDTO.KillCount;
        _progressUI.UpdateProgressSlider();
    }

    public void LoadInventoryData()
    {
        InventoryData.Init();

        for (int i = 0; i < LoadData.InventoryDataDTO.Items.Count; i++)
        {
            ItemDTO itemDTO = LoadData.InventoryDataDTO.Items[i];
            ItemSO itemSO = itemDTO.Item;
            EquipItemSO equipItemSO = itemSO as EquipItemSO;

            if (itemDTO.ItemIndex != 0)
            {
                EquipItemSO LoadItem = Resources.Load<EquipItemSO>($"Item/Equip/{equipItemSO.EquipmentType}/{equipItemSO.EquipmentType}_{itemDTO.ItemIndex}");
                if (LoadItem != null)
                {
                    InventoryData.AddItem(new InventoryItem
                    {
                        Item = LoadItem,
                        Quantity = itemDTO.Quantity
                    });
                }
                else
                {
                    Debug.LogWarning($"������ ���ҽ��� ã�� �� �����ϴ�: {equipItemSO.EquipmentType}_{itemDTO.ItemIndex}");
                }
            }

        }
    }
}
