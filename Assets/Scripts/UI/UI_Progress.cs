using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Progress : UIBinder
{
    private Slider _progressSlider => GetUI<Slider>("ProgressSlider");

    private TMP_Text _progressText => GetUI<TMP_Text>("ProgressText");

    private float _lerpSpeed = 1.0f;

    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        ChapterManager.Instance.ProgressInfo.OnStageChanged += UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnChapterChanged += UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnKillCountChanged += UpdateProgressSlider;

        Init();
    }

    private void OnDestroy()
    {
        ChapterManager.Instance.ProgressInfo.OnStageChanged -= UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnChapterChanged -= UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnKillCountChanged -= UpdateProgressSlider;
    }

    private void Init()
    {
        UpdateProgressUI();

        _progressSlider.maxValue = 5;
        _progressSlider.value = ChapterManager.Instance.ProgressInfo.KillCount;
    }

    public void UpdateProgressUI()
    {
        _progressText.text = $"{ChapterManager.Instance.ProgressInfo.Chapter} - {ChapterManager.Instance.ProgressInfo.Stage}";
    }

    public void UpdateProgressSlider()
    {
        StartCoroutine(SliderRoutine());
    }

    IEnumerator SliderRoutine()
    {
        float targetValue = ChapterManager.Instance.ProgressInfo.KillCount;

        do
        {
            _progressSlider.value = Mathf.Lerp(_progressSlider.value, targetValue, Time.deltaTime * _lerpSpeed);
        } while (_progressSlider.value == targetValue);

        yield return null;
    }
}
