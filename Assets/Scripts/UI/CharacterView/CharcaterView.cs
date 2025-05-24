using System;
using TMPro;
using UnityEngine;

public class CharcaterView : UIBinder
{
    [SerializeField] CharacterModel _model;

    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        Init();
    }

    #region Subscrigbe/UnSubscribe/Init
    public void Init()
    {
        GetUI<TMP_Text>("CurHPText").text = Util.GetText($"Hp : {_model.CurHp}").ToString();
        GetUI<TMP_Text>("MaxHpText").text = Util.GetText($"Max Hp : {_model.MaxHp}").ToString();
        GetUI<TMP_Text>("RecoverHpText").text = Util.GetText($"RecoveryHp/S : {_model.RerecoverHpPerSecond}").ToString();

        GetUI<TMP_Text>("DefensePowerText").text = Util.GetText($"Defense : {_model.DefensePower}").ToString();

        GetUI<TMP_Text>("AttackPowerText").text = Util.GetText($"AttackPower : {_model.AttackPower}").ToString();
        GetUI<TMP_Text>("AttackSpeedText").text = Util.GetText($"AttackSpeed : {_model.AttackSpeed}").ToString();
        GetUI<TMP_Text>("CriticalChanceText").text = Util.GetText($"Critical : {_model.CriticalChacnce}").ToString();
    }
    #endregion

    #region CallBack Method
    public void UpdateCurHp() => GetUI<TMP_Text>("CurHPText").text = Util.GetText($"Hp : {_model.CurHp}").ToString();
    public void UpdateMaxHp() => GetUI<TMP_Text>("MaxHpText").text = Util.GetText($"MaxHp : {_model.MaxHp}").ToString();
    public void UpdateRerecoverHpPerSecond() => GetUI<TMP_Text>("RecoverHpText").text = Util.GetText($"RecoveryHp/S : {_model.RerecoverHpPerSecond}").ToString();
    public void UpdateDefensePower() => GetUI<TMP_Text>("DefensePowerText").text = Util.GetText($"Defense : {_model.DefensePower}").ToString();
    public void UpdateAttackPower() => GetUI<TMP_Text>("AttackPowerText").text = Util.GetText($"AttackPower : {_model.AttackPower}").ToString();
    public void UpdateAttackSpeed() => GetUI<TMP_Text>("AttackSpeedText").text = Util.GetText($"AttackSpeed : {_model.AttackSpeed}").ToString();
    public void UpdateCriticalChance() => GetUI<TMP_Text>("CriticalChanceText").text = Util.GetText($"Critical : {_model.CriticalChacnce}").ToString();

   // public void UpdateSkillDamage() => GetUI<TMP_Text>("SkillDamage").text = Util.GetText($"{_model.SkillDamage}").ToString();
   // public void UpdateSkillInterval() => GetUI<TMP_Text>("SkillInterval").text = Util.GetText($"{_model.SkillInterval}").ToString();
    #endregion
}
