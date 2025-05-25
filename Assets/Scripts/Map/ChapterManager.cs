using System.Collections;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance { get; private set; }

    [SerializeField] ParallaxBackground _backGround;

    [SerializeField] CharacterController _characterController;

    public DataManager Data;

    [Header("")]
    [SerializeField] ProgressSO _progressInfo;
    public ProgressSO ProgressInfo { get { return _progressInfo; } set { _progressInfo = value; } }


    private void Awake() => SetSingleton();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("1");
            ClearChapter();
        }
    }
    private void SetSingleton()
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

    private void OnEnable()
    {
        ProgressInfo.OnClearStage += ClearStage;
    }

    private void OnDisable()
    {
        ProgressInfo.OnClearStage -= ClearStage;
    }

    /// <summary>
    /// Stage에 맞는 몬스터 정보 초기화
    /// 메서드의 매개변수는 stageNum
    /// </summary>
    /// <param name="stageNum"></param>
    /*private void Init(int chapter, int stage)
    {
        ProgressInfo.Stage = int.Parse(Data.MonsterCSV.GetData(stageNum, (int)MonsterData.Stage));
    }*/


    /// <summary>
    /// 챕터 클리어 시, 다음 챕터로
    /// FadiIn 효과
    /// </summary>
    public void ClearChapter()
    {
        ProgressInfo.Chapter++;
        ProgressInfo.Stage = 1;

        CameraUtil.CameraFadeIn();
        DelayLayerMoveSpeed();
    }

    /// <summary>
    /// 챕터 속 스테이지 클리어 시, 다음 스테이지로
    /// FadeIn 효과
    /// </summary>
    public void ClearStage()
    {
        ProgressInfo.Stage++;

        CameraUtil.CameraFadeIn();
        DelayLayerMoveSpeed();
    }


    /// <summary>
    /// 딜레이 주고 LayerMoveSpeed 초기화
    /// </summary>
    /// <returns></returns>
    Coroutine DelayLayerMoveSpeedRoutine;
    IEnumerator DelayLayerMoveSpeed()
    {
        yield return Util.GetDelay(2f);

        _backGround.SetLayerMoveSpeed();
    }
}