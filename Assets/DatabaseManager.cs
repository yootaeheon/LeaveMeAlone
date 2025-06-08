using Firebase.Database;
using System;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Extensions;


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
            Debug.Log("부른다!!");
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
        string userId = BackendManager.Auth?.CurrentUser?.UserId;
        Debug.Log("LOAD ALL GAMEDATA 2");
        if (string.IsNullOrEmpty(userId)) return;
        Debug.Log("LOAD ALL GAMEDATA 3");

        BackendManager.Database.RootReference.Child(userId).Child("gameData").GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    Debug.Log($"_model: {_model}, _progressData: {_progressData}");
                    Debug.Log("LOAD ALL GAMEDATA 4");
                    if (_model == null || _progressData == null) return;
                    Debug.Log("LOAD ALL GAMEDATA 5");
                    string json = task.Result.GetRawJsonValue();
                    Debug.Log("LOAD ALL GAMEDATA 6");
                    GameData = JsonUtility.FromJson<UserGameDataDTO>(json);
                    Debug.Log("LOAD ALL GAMEDATA 7");

                    Model.MaxHp = GameData.CharacterModelDTO.MaxHp;
                    Model.CurHp = Model.MaxHp;
                    Model.RecoverHpPerSecond = GameData.CharacterModelDTO.RecoverHpPerSecond;
                    Model.DefensePower = GameData.CharacterModelDTO.DefensePower;
                    Model.AttackPower = GameData.CharacterModelDTO.AttackPower;
                    Model.AttackSpeed = GameData.CharacterModelDTO.AttackSpeed;
                    Model.CriticalChance = GameData.CharacterModelDTO.CriticalChance;


                    Debug.Log("LOAD ALL GAMEDATA 8");
                    Debug.Log($"{ProgressData}이 널이냐 제발 알려줘");
                    ProgressData.Chapter = GameData.ProgressDataDTO.Chapter;
                    Debug.Log("LOAD ALL GAMEDATA 8-1");
                    ProgressData.Stage = GameData.ProgressDataDTO.Stage;
                    Debug.Log("LOAD ALL GAMEDATA 8-2");
                    ProgressData.KillCount = GameData.ProgressDataDTO.KillCount;
                    Debug.Log("LOAD ALL GAMEDATA 9");
                    IsGameDataLoaded = true;
                    OnGameDataLoaded?.Invoke();
                    Debug.Log("LOAD ALL GAMEDATA 10");

                    Debug.Log("모든 게임 데이터 불러오기 완료!");
                }
                /* if (task.IsCompleted && task.Result.Exists)
                 {
                     var snapshot = task.Result;

                     var characterModel = snapshot.Child("CharacterModelDTO");
                     Model.MaxHp = Convert.ToInt32(characterModel.Child("MaxHp").Value);
                     Model.CurHp = Model.MaxHp;
                     Debug.Log("잘되가는중 1");
                     Model.RecoverHpPerSecond = Convert.ToSingle(characterModel.Child("RecoverHpPerSecond").Value);
                     Model.DefensePower = Convert.ToInt32(characterModel.Child("DefensePower").Value);
                     Model.AttackPower = Convert.ToInt32(characterModel.Child("AttackPower").Value);
                     Model.AttackSpeed = Convert.ToSingle(characterModel.Child("AttackSpeed").Value);
                     Model.CriticalChance = Convert.ToSingle(characterModel.Child("CriticalChance").Value);

                     var progressData = snapshot.Child("ProgressDataDTO");
                     ProgressData.Chapter = Convert.ToInt32(progressData.Child("Chapter").Value);
                     ProgressData.Stage = Convert.ToInt32(progressData.Child("Stage").Value);
                     ProgressData.KillCount = Convert.ToInt32(progressData.Child("KillCount").Value);

                     OnGameDataLoaded?.Invoke();
                     Debug.Log("LOAD ALL GAMEDATA 10");

                     Debug.Log("직접 접근 방식으로 데이터 로딩 완료");
                 }*/
                else
                {
                    Debug.LogWarning("게임 데이터 불러오기 실패: " + task.Exception);
                }
            });
    }
    #endregion
}
