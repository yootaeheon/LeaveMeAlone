using Firebase.Database;
using System;
using UnityEngine;
using UnityEngine.Events;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    public UserGameDataDTO GameData;

    public DatabaseReference userDataRef { get; private set; }


    [SerializeField] CharacterModel _model;
    public CharacterModel Model => _model ??= FindAnyObjectByType<CharacterModel>();

    [SerializeField] ProgressSO _progressData;
    public ProgressSO ProgressData => _progressData ??= FindAnyObjectByType<ChapterManager>()?.ProgressInfo;


    public bool IsGameDataLoaded { get; private set; }
    public Action OnGameDataLoaded { get; set; }

    private void Awake()
    {
        SetSingleton();
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

    public void SaveAllGameData()
    {
        string userId = BackendManager.Auth?.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(userId)) return;

        if (Model == null)
        {
            Debug.LogWarning("SaveAllGameData 실패: _model이 null입니다.");
            return;
        }

        if (ProgressData == null)
        {
            Debug.LogWarning("SaveAllGameData 실패: _progressData가 null입니다.");
            return;
        }

        userDataRef = BackendManager.Database.GetReference("users").Child(userId);

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

        GameData = new UserGameDataDTO(characterDTO, progressDTO);
        string json = JsonUtility.ToJson(GameData);

        userDataRef.Child("gameData")
            .SetRawJsonValueAsync(json)
            .ContinueWith(task =>
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
        Debug.Log("LOAD ALL GAMEDATA 1");
        string uid = BackendManager.Auth?.CurrentUser?.UserId;
        Debug.Log("LOAD ALL GAMEDATA 2");
        if (string.IsNullOrEmpty(uid)) return;
        Debug.Log("LOAD ALL GAMEDATA 3");

        BackendManager.Database
            .GetReference("users").Child(uid).Child("gameData")
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    if (_model == null || _progressData == null) return;

                    string json = task.Result.GetRawJsonValue();
                    GameData = JsonUtility.FromJson<UserGameDataDTO>(json);


                    Model.MaxHp = GameData.CharacterModelDTO.MaxHp;
                    Model.RecoverHpPerSecond = GameData.CharacterModelDTO.RecoverHpPerSecond;
                    Model.DefensePower = GameData.CharacterModelDTO.DefensePower;
                    Model.AttackPower = GameData.CharacterModelDTO.AttackPower;
                    Model.AttackSpeed = GameData.CharacterModelDTO.AttackSpeed;
                    Model.CriticalChance = GameData.CharacterModelDTO.CriticalChance;

                    ProgressData.Chapter = GameData.ProgressDataDTO.Chapter;
                    ProgressData.Stage = GameData.ProgressDataDTO.Stage;
                    ProgressData.KillCount = GameData.ProgressDataDTO.KillCount;

                    IsGameDataLoaded = true;
                    OnGameDataLoaded.Invoke();


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