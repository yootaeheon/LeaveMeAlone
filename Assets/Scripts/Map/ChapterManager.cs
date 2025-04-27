using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance {  get; private set; }

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

    public void ClearChapter()
    {
        //TODO : 화면 어두워졌다 밝아지면서 다시 시작점으로 오는 애니메이션
        // 캐릭터 움직임X 카메라만 샘플 씬 애니메이션 가져오기
        // 화면 밝아지면 몇 초 뒤 화면 스크롤링 시작
        DelayLayerMoveSpeed();
    }


    public void ClearStage()
    {
        //TODO : 화면 어두워졌다 밝아지면서 다시 시작점으로 오는 애니메이션
        // 캐릭터 움직임X 카메라만 샘플 씬 애니메이션 가져오기
        // 화면 밝아지면 몇 초 뒤 화면 스크롤링 시작
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