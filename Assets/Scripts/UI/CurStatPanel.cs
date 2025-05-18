using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CurStatPanel : UIBinder
{
    private CharacterModel _characterModel;
    
    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        _characterModel = FindAnyObjectByType<CharacterModel>();
    }

    public void Init()
    {
        GetUI<TMP_Text>("CurHPText").text = Util.GetText($"{_characterModel.CurHp}").ToString();
        GetUI<TMP_Text>("MaxHpText").text = Util.GetText($"{_characterModel.MaxHp}").ToString();
        GetUI<TMP_Text>("RecoverHpText").text = Util.GetText($"{_characterModel.RerecoverHpPerSecond}").ToString();

        GetUI<TMP_Text>("DefensePowerText").text = Util.GetText("sda").ToString();

        GetUI<TMP_Text>("AttackPowerText").text = Util.GetText("sda").ToString();
        GetUI<TMP_Text>("AttackSpeedText").text = Util.GetText("sda").ToString();
        GetUI<TMP_Text>("CriticalChanceText").text = Util.GetText("sda").ToString();
    }
}
