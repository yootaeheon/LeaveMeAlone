using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptables/SoundDataSO")]
public class SoundDatSO : ScriptableObject
{
    [SerializeField] private Sound _sound;

    /// <summary>
    /// BGM
    /// </summary>
    public AudioClip TitleBGM { get { return _sound.TitleBGM; } }
    public AudioClip LobbyBGM { get { return _sound.LobbyBGM; } }
    public AudioClip InGameBGM { get { return _sound.InGameBGM; } }
    public AudioClip GameOver { get { return _sound.GameOver; } }
    public AudioClip GameClear { get { return _sound.GameClear; } }
  
    /// <summary>
    /// UI 사운드
    /// </summary>
    public AudioClip OnUI { get { return _sound.OnUI; } }
    public AudioClip OffUI { get { return _sound.OffUI; } }
    public AudioClip MoveUI { get { return _sound.MoveUI; } }
    public AudioClip SelectUI { get { return _sound.SelectUI; } }
    public AudioClip ApplyUI { get { return _sound.ApplyUI; } }

    [Serializable]
    public struct Sound
    {
        [Header("BGM")]
        public AudioClip TitleBGM;
        public AudioClip LobbyBGM;
        public AudioClip InGameBGM;
        public AudioClip GameOver;
        public AudioClip GameClear;
        public AudioClip HPLowBGM;
        public AudioClip StoreBGM;

        [Header("SFX")]
        [Header("UI")]
        public AudioClip OnUI;               // UI 켜기
        public AudioClip OffUI;              // UI 닫기
        public AudioClip MoveUI;             // UI 이동
        public AudioClip SelectUI;           // UI 선택
        public AudioClip ApplyUI;            // UI 적용
    }
}