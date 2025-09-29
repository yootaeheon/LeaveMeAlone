using Firebase.Database;
using Firebase.Extensions;
using Inventory.Model;
using System;
using UnityEngine;



/*#if UNITY_EDITOR
using UnityEditor;
#endif*/

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

    [SerializeField] EquipmentManager _equipmentManager;
    public EquipmentManager EquipmentManager => _equipmentManager ??= FindAnyObjectByType<EquipmentManager>();

    public bool IsGameDataLoaded { get; private set; }
    public Action OnGameDataLoaded { get; set; }

    private void Awake()
    {
        SetSingleton();

        // 모든 플랫폼에서 앱 종료 감지
        Application.quitting += OnApplicationQuit;
    }

    private void OnDestroy()
    {
        Application.quitting -= OnApplicationQuit;
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

    /*#if UNITY_EDITOR
        private void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                SaveAllGameData();
                Debug.Log("에디터 실행 종료 시 SaveAllGameData 호출");
            }
        }
    #endif*/

    /// <summary>
    /// 데이터 저장
    /// </summary>
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

        InventoryDTO inventoryDataDTO = new InventoryDTO(InventoryData, EquipmentManager._equippedItems);

        GoldDataDTO goldDataDTO = new GoldDataDTO
        (
            GameManager.Instance.Gold,
            GameManager.Instance.Gem
        );

        EquipmentDTO equipmentDTO = new EquipmentDTO
        (
            _equipmentManager._equippedItems
        );

        LoadData = new UserGameDataDTO(characterDTO, progressDTO, inventoryDataDTO, goldDataDTO, equipmentDTO);
        string json = JsonUtility.ToJson(LoadData);

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

    /// <summary>
    /// 모든 데이터 불러오기 
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
                    LoadGoldData();
                    //LoadEquipmentData();

                    IsGameDataLoaded = true;
                    OnGameDataLoaded?.Invoke();

                    Debug.Log("모든 게임 데이터 불러오기 완료!");
                }
                else
                {
                    InitNewGameData();
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

        Debug.Log("1. 플레이어 모델 데이터 로드 완료");
    }

    public void LoadProgressData()
    {
        ProgressData.Chapter = LoadData.ProgressDataDTO.Chapter;
        ProgressData.Stage = LoadData.ProgressDataDTO.Stage;
        ProgressData.KillCount = LoadData.ProgressDataDTO.KillCount;
        _progressUI.UpdateProgressSlider();

        Debug.Log("2. 진행도 로드 완료");
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
                    Debug.LogWarning($"아이템 리소스를 찾을 수 없습니다: {equipItemSO.EquipmentType}_{itemDTO.ItemIndex}");
                }
            }
        }
        Debug.Log("3. 인벤토리 로드 완료");

        if (LoadData.InventoryDataDTO.EquipItems != null)
        {
            foreach (var equipDTO in LoadData.InventoryDataDTO.EquipItems)
            {
                ItemDTO itemDTO = LoadData.InventoryDataDTO.EquipItems.Find(i => i.ItemIndex == equipDTO.ItemIndex);
                ItemSO itemSO = itemDTO.Item;
                EquipItemSO equipItemSO = itemSO as EquipItemSO;

                if (equipDTO.Item != null && equipDTO.ItemIndex != 0)
                {
                    EquipItemSO LoadItem = Resources.Load<EquipItemSO>($"Item/Equip/{equipItemSO.EquipmentType}/{equipItemSO.EquipmentType}_{itemDTO.ItemIndex}");
                    if (LoadItem != null)
                    {
                        EquipmentManager._equippedItems[LoadItem.EquipmentType] = LoadItem;
                    }
                    else
                    {
                        Debug.LogWarning("장비 리소스를 찾을 수 없습니다: " + equipItemSO.EquipmentType + "_" + itemDTO.ItemIndex);
                    }
                }
            }
        }
        EquipmentManager.ApplyAllEquippedItemsToCharacter();
    }

    public void LoadGoldData()
    {
        GameManager.Instance.Gold = LoadData.GoldDataDTO.Gold;
        GameManager.Instance.Gem = LoadData.GoldDataDTO.Gem;

        Debug.Log("4. 재화 데이터 로드 완료");
    }

    /*public void LoadEquipmentData()
    {
        if (LoadData.EquipmentDTO?.EquippedItems == null) return;

        EquipmentManager._equippedItems.Clear();
        foreach (var slot in LoadData.EquipmentDTO.EquippedItems)
        {
            EquipItemSO item = Resources.Load<EquipItemSO>($"Item/Equip/{slot.Type}/{slot.Type}_{slot.ItemID}");
            if (item != null)
            {
                EquipmentManager._equippedItems[slot.Type] = item;
            }
            else
            {
                Debug.LogWarning($"장비 리소스를 찾을 수 없습니다: {slot.Type}_{slot.ItemID}");
            }
        }

        EquipmentManager.ApplyAllEquippedItemsToCharacter();

        Debug.Log("5. 장비 데이터 로드 및 적용 완료");
    }*/

    public void InitNewGameData()
    {
        Debug.Log("새 유저 → 기본 데이터 생성");

        // 기본 캐릭터 값 지정
        Model.MaxHp = 100;
        Model.CurHp = 100;
        Model.AttackPower = 10;
        Model.DefensePower = 5;
        Model.AttackSpeed = 1.0f;
        Model.CriticalChance = 0.1f;

        ProgressData.Chapter = 1;
        ProgressData.Stage = 1;
        ProgressData.KillCount = 0;

        GameManager.Instance.Gold = 0;
        GameManager.Instance.Gem = 0;

        InventoryData.Init();

        IsGameDataLoaded = true;
        OnGameDataLoaded?.Invoke();

        SaveAllGameData(); // Firebase에 저장
    }
}
