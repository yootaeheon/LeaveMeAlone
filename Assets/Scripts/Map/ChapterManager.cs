using System.Collections;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance { get; private set; }

    [SerializeField] ParallaxBackground _backGround;

    public StageInfo[] StageInfos { get; private set; }


    private void Awake()
    {
        SetSingleton();
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