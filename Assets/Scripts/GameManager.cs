using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] TMP_Text _goldText;
    [SerializeField] TMP_Text _gemText;

    [SerializeField] int _gold;
    public int Gold { get { return _gold; } set { _gold = value; UpdateGold(); } }

    [SerializeField] int _gem;
    public int Gem {  get { return _gem; } set {  _gem = value; UpdateGem(); } }

    public List<float> floats = new List<float>(2);



    private void Awake()
    {
        SetSingleton();
    }

    #region ΩÃ±€≈Ê ºº∆√
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
    #endregion

    private void Start()
    {
        SoundManager.PlayBGM(SoundManager.SoundData.InGameBGM);

        Init();
    }

    private void Init()
    {
        UpdateGold();
        UpdateGem();
    }

    

    private void UpdateGold()
    {
        _goldText.text = $"{Gold}";
    }

    private void UpdateGem()
    {
        _gemText.text = $"{Gem}";
    }
}
