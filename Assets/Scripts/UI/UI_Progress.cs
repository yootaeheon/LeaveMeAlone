using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_Progress : UIBinder
{
    public Slider _progressSlider;

    private TMP_Text _progressText;

    [SerializeField] ProgressSO _progressData;

    private void Awake()
    {
        BindAll();
        
        /*_progressSlider = GetUI<Slider>("ProgressSlider");*/

        _progressText = GetUI<TMP_Text>("ProgressText");
    }

    private void Start()
    {
        ChapterManager.Instance.ProgressInfo.OnStageChanged += UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnChapterChanged += UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnKillCountChanged += () => UpdateProgressSlider();


        Init();
    }

    private void OnDestroy()
    {
        ChapterManager.Instance.ProgressInfo.OnStageChanged -= UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnChapterChanged -= UpdateProgressUI;
        ChapterManager.Instance.ProgressInfo.OnKillCountChanged -= () => UpdateProgressSlider();
    }

    private void Init()
    {
        UpdateProgressUI();

        _progressSlider.maxValue = 5;
    }

    public void UpdateProgressUI()
    {
        _progressText.text = $"{ChapterManager.Instance.ProgressInfo.Chapter} - {ChapterManager.Instance.ProgressInfo.Stage}";
    }

    public void UpdateProgressSlider()
    {
        /*float targetValue = ChapterManager.Instance.ProgressInfo.KillCount;
        _progressSlider.value = Mathf.Lerp(_progressSlider.value, targetValue, Time.deltaTime * _lerpSpeed);*/
        _progressSlider.value = ChapterManager.Instance.ProgressInfo.KillCount;
        Debug.Log("»£√‚«ﬂ¥Ÿ¿◊");
    }
}
