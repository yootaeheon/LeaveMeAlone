using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Progress : UIBinder
{
    private void Awake()
    {
       BindAll();
    }

    private void Start()
    {
        Init();

        ChapterManager.Instance.ProgressInfo.OnStageChanged += UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnChapterChanged += UpdateProgressUI;
    }

    private void OnDestroy()
    {
        ChapterManager.Instance.ProgressInfo.OnStageChanged -= UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnChapterChanged -= UpdateProgressUI;
    }

    private void Init()
    {
        UpdateProgressUI();
    }

    public void UpdateProgressUI()
    {
        GetUI<TMP_Text>("ProgressText").text = $"{ChapterManager.Instance.ProgressInfo.Chapter} - {ChapterManager.Instance.ProgressInfo.Stage}";
    }
}
