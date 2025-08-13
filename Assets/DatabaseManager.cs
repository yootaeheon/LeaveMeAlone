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

        // 에디터 모드에서 종료 감지
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

    #region 싱글톤 세팅
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
            Debug.Log("에디터 실행 종료 시 SaveAllGameData 호출");
        }
    }
#endif

    public void SaveAllGameData()
    {
        string userId = BackendManager.Auth?.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(userId)) return;

        if (Model == null || ProgressData == null || InventoryData == null)
        {
            Debug.LogWarning("SaveAllGameData 실패: 무언가 Null임.");
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
                    Debug.Log("모든 게임 데이터 저장 완료!");
                else
                    Debug.LogWarning("게임 데이터 저장 실패: " + task.Exception);
            });
    }

    #region 모든 데이터 업데이트
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

                    // 캐릭터 데이터
                    Model.MaxHp = GameData.CharacterModelDTO.MaxHp;
                    Model.CurHp = Model.MaxHp;
                    Model.RecoverHpPerSecond = GameData.CharacterModelDTO.RecoverHpPerSecond;
                    Model.DefensePower = GameData.CharacterModelDTO.DefensePower;
                    Model.AttackPower = GameData.CharacterModelDTO.AttackPower;
                    Model.AttackSpeed = GameData.CharacterModelDTO.AttackSpeed;
                    Model.CriticalChance = GameData.CharacterModelDTO.CriticalChance;
                    Debug.Log("모델 데이터 불러오기 완료!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                    // 진행도 데이터
                    ProgressData.Chapter = GameData.ProgressDataDTO.Chapter;
                    ProgressData.Stage = GameData.ProgressDataDTO.Stage;
                    ProgressData.KillCount = GameData.ProgressDataDTO.KillCount;
                    _progressUI.UpdateProgressSlider();

                    //인벤토리 데이터
                    // 인벤토리 데이터 복원
                    InventoryData.Init(); // 먼저 비움
                    for (int i = 0; i < GameData.InventoryDataDTO.Items.Count; i++)
                    {
                        var itemDTO = GameData.InventoryDataDTO.Items[i];
                        if (itemDTO.ItemID != 0) // 0이면 빈 슬롯
                        {
                            // ItemSO 리소스 로드 (경로나 이름 규칙 필요)
                            ItemSO itemSO = Resources.Load<ItemSO>($"Items/{itemDTO.ItemID}");
                            if (itemSO != null)
                            {
                                InventoryData.RemoveItem(i, InventoryData.GetItemIndex(i).Quantity); // 기존 제거
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

                    Debug.Log("모든 게임 데이터 불러오기 완료!");
                }
                else
                {
                    Debug.LogWarning("게임 데이터 불러오기 실패: " + task.Exception);
                }
            });
    }
    #endregion
}
