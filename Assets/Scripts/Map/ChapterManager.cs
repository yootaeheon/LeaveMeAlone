using System.Collections;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance { get; private set; }

    [SerializeField] ParallaxBackground _backGround;

    [SerializeField] CharacterController _characterController;

    public StageInfo[] StageInfos { get; private set; }


    private void Awake()
    {
        SetSingleton();
    }

    /// <summary>
    /// 스테이지 클리어 시 FadeIn 필요 없을 시 구독 삭제할 것
    /// </summary>
   /* private void OnEnable()
    {
        _characterController.OnKillMonster += ClearStage;
    }

    private void OnDisable()
    {
        _characterController.OnKillMonster -= ClearStage;
    }*/

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
    /// 챕터 클리어 시, 다음 챕터로
    /// </summary>
    public void ClearChapter()
    {
        CameraUtil.CameraFadeIn();
        DelayLayerMoveSpeed();
    }

    /// <summary>
    /// 챕터 속 스테이지 클리어 시, 다음 스테이지로
    /// </summary>
    public void ClearStage()
    {
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

public class StageInfo
{
    public int Stage { get; private set; }
}