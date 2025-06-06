using UnityEngine;
using UnityEngine.UI;

public class UI_SettingCanvas : MonoBehaviour
{
    [SerializeField] GameObject _settingCanvas;

    [Header("Volume")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Slider _BGMSlider;

    [Header("TargetFrame")]
    [SerializeField] GameObject[] _frameRateChecks;



    void Start()
    {
        // 저장된 볼륨 불러오기 (기본값은 1.0)
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        _BGMSlider.value = savedVolume;
        _audioSource.volume = savedVolume;

        // 슬라이더 값이 바뀔 때마다 볼륨 적용
        _BGMSlider.onValueChanged.AddListener(SetVolume);
    }

    public void Button_ShowSettingCanvas()
    {
        _settingCanvas.SetActive(true);
    }

    public void Button_HideSettingCanvas()
    { 
        _settingCanvas.SetActive(false); 
    }

    public void Button_Frame30()
    {
        Application.targetFrameRate = 30;
        foreach (var frameRate in _frameRateChecks)
        {
            frameRate.SetActive(false);
        }
        _frameRateChecks[0].SetActive(true);
    }

    public void Button_Frame60()
    {
        Application.targetFrameRate = 60;
        foreach (var frameRate in _frameRateChecks)
        {
            frameRate.SetActive(false);
        }
        _frameRateChecks[1].SetActive(true);
    }

    public void Button_FrameX()
    {
        Application.targetFrameRate = -1;
        foreach (var frameRate in _frameRateChecks)
        {
            frameRate.SetActive(false);
        }
        _frameRateChecks[2].SetActive(true);
    }

    public void SetVolume(float value)
    {
        _audioSource.volume = value;

        // 볼륨 값 저장
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
