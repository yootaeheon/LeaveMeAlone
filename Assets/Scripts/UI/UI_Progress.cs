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
        _progressData.OnStageChanged += UpdateProgressUI;
        _progressData.OnChapterChanged += UpdateProgressUI;
        _progressData.OnKillCountChanged += () => UpdateProgressSlider();


        Init();
    }

    private void OnDestroy()
    {
        _progressData.OnStageChanged -= UpdateProgressUI;
        _progressData.OnChapterChanged -= UpdateProgressUI;
        _progressData.OnKillCountChanged -= () => UpdateProgressSlider();
    }

    private void Init()
    {
        UpdateProgressUI();

        _progressSlider.maxValue = 5;
    }

    public void UpdateProgressUI()
    {
        _progressText.text = $"{_progressData.Chapter} - {_progressData.Stage}";
    }

    public void UpdateProgressSlider()
    {
        /*float targetValue = ChapterManager.Instance.ProgressInfo.KillCount;
        _progressSlider.value = Mathf.Lerp(_progressSlider.value, targetValue, Time.deltaTime * _lerpSpeed);*/
        _progressSlider.value = _progressData.KillCount;
        Debug.Log("»£√‚«ﬂ¥Ÿ¿◊");
    }
}
