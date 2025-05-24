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

    /// <summary>
    /// Stage에 맞는 몬스터 정보 초기화
    /// 메서드의 매개변수는 stageNum
    /// </summary>
    /// <param name="stageNum"></param>
    private void Init(int stageNum)
    {
        ProgressInfo.MonsterNumInStage = int.Parse(Data.MonsterCSV.GetData(stageNum, (int)MonsterData.Num));
    }


    /// <summary>
    /// 챕터 클리어 시, 다음 챕터로
    /// FadiIn 효과
    /// </summary>
    public void ClearChapter()
    {
        _progressInfo.Chapter++;
        _progressInfo.Stage = 1;

        CameraUtil.CameraFadeIn();
        DelayLayerMoveSpeed();
    }

    /// <summary>
    /// 챕터 속 스테이지 클리어 시, 다음 스테이지로
    /// FadeIn 효과
    /// </summary>
    public void ClearStage()
    {
        _progressInfo.Stage++;

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


    // 만들어야할 기능
    // 현재 ProgressSO에 현재 진행도 저장
    // stage 클리어 시 curStage++; 
    // chapter 클리어 시 chapter++; curStage = 1; 
    // (1챕터에 10 스테이지까지 있음)
    // Stage와 Chapter 클리어 시 FadeIn 효과
    // chapterManager에서 이벤트를 가지고 있고 스테이지와 챕터 클리어 시 OnStageClear?.Invoke(); OnChapterClear?.Invoke();
    // 클리어할때마다 MonsterData를 업데이트해서 몬스터에 불어넣어줌
    // 아래 코드를 이용하여 Init(_progressInfo.stage); 이용하여 이벤트호출할때 함수도 호출
    // 
    /*private void Init(int stageNum)
    {
        ProgressInfo.MonsterNumInStage = int.Parse(Data.MonsterCSV.GetData(stageNum, (int)MonsterData.Num));
    }*/
}