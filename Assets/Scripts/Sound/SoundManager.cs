using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

/*    const string SOUND_PATH = "Resources/Sound";*/


    [SerializeField] SoundDatSO _soundData;
    public static SoundDatSO SoundData { get { return Instance._soundData; } private set { } }


    // BGM 소스
    [SerializeField] private AudioSource bgmSource;
    public static AudioSource BGM { get { return Instance.bgmSource; } }

    // SFX 소스
    [SerializeField] private AudioSource sfxSource;
    public static AudioSource SFX { get { return Instance.sfxSource; } }

    /// <summary>
    /// 사운드 매니저를 싱글톤으로 선언                              
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

           /* Init();*/
        }
        else
        {
            Destroy(gameObject);
        }

    }

  /*  private void Init()
    {
        _soundData = Resources.Load<SoundDatSO>($"{SOUND_PATH}/SoundData");
    }*/

    /// <summary>
    /// 배경 음악 교체 후 재생
    /// </summary>
    public static void PlayBGM(AudioClip clip)
    {
        if (clip == null)
            return;

        if (BGM.isPlaying)
        {
            BGM.Stop();
        }
        
        BGM.clip = clip;
        BGM.loop = true;
        BGM.Play();
    }

    /// <summary>
    /// 배경 음악 정지
    /// </summary>
    public static void StopBGM()
    {
        if (BGM.isPlaying == false)
            return;

        BGM.Stop();
    }

    /// <summary>
    /// 효과음 교체 후 재생
    /// </summary>
    public static void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        SFX.clip = clip;
        SFX.PlayOneShot(clip);
    }

    /// <summary>
    /// 효과음 정지
    /// </summary>
    public static void StopSFX()
    {
        if (SFX.isPlaying == false)
            return;

        SFX.Stop();
    }
}