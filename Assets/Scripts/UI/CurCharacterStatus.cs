using System;
using TMPro;
using UnityEngine;

public class CurCharacterStatus : UIBinder
{
    [SerializeField] CharacterModel _characterModel;

    private void Awake()
    {
        BindAll();

        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    public void Subscribe()
    {
        _characterModel.CurHpChanged += UpdateCurHp;
        _characterModel.MaxHpChanged += UpdateMaxHp;
        _characterModel.RerecoverHpPerSecondChanged += UpdateRerecoverHpPerSecond;
        _characterModel.DefensePowerChanged += UpdateDefensePower;
        _characterModel.AttackPowerChanged += UpdateAttackPower;
        _characterModel.AttackSpeedChanged += UpdateAttackSpeed;
        _characterModel.CriticalChacnceChanged += UpdateCriticalChance;
        //_characterModel.SkillDamageChanged += 
        //_characterModel.SkillIntervalChanged +=
    }

    public void UnSubscribe()
    {
        _characterModel.CurHpChanged -= UpdateCurHp;
        _characterModel.MaxHpChanged -= UpdateMaxHp;
        _characterModel.RerecoverHpPerSecondChanged -= UpdateRerecoverHpPerSecond;
        _characterModel.DefensePowerChanged -= UpdateDefensePower;
        _characterModel.AttackPowerChanged -= UpdateAttackPower;
        _characterModel.AttackSpeedChanged -= UpdateAttackSpeed;
        _characterModel.CriticalChacnceChanged -= UpdateCriticalChance;
            //_characterModel.SkillDamageChanged -= 
        //_characterModel.SkillIntervalChanged -=
    }

    private void Start()
    {
        /*_characterModel = FindAnyObjectByType<CharacterModel>();*/
        Init();
    }

    public void Init()
    {
        GetUI<TMP_Text>("CurHPText").text = Util.GetText($"Cur Hp : {_characterModel.CurHp}").ToString();
        GetUI<TMP_Text>("MaxHpText").text = Util.GetText($"Max Hp : {_characterModel.MaxHp}").ToString();
        GetUI<TMP_Text>("RecoverHpText").text = Util.GetText($"RecoveryHp/S : {_characterModel.RerecoverHpPerSecond}").ToString();

        GetUI<TMP_Text>("DefensePowerText").text = Util.GetText($"DefensePower : {_characterModel.DefensePower}").ToString();

        GetUI<TMP_Text>("AttackPowerText").text = Util.GetText($"AttackPower : {_characterModel.AttackPower}").ToString();
        GetUI<TMP_Text>("AttackSpeedText").text = Util.GetText($"AttackSpeed : {_characterModel.AttackSpeed}").ToString();
        GetUI<TMP_Text>("CriticalChanceText").text = Util.GetText($"Critical : {_characterModel.CriticalChacnce}").ToString();
    }

    #region CallBack Method
    public void UpdateCurHp() => GetUI<TMP_Text>("CurHPText").text = Util.GetText($"{_characterModel.CurHp}").ToString();
    public void UpdateMaxHp() => GetUI<TMP_Text>("MaxHpText").text = Util.GetText($"{_characterModel.MaxHp}").ToString();
    public void UpdateRerecoverHpPerSecond() => GetUI<TMP_Text>("RecoverHpText").text = Util.GetText($"{_characterModel.RerecoverHpPerSecond}").ToString();

    public void UpdateDefensePower() => GetUI<TMP_Text>("DefensePowerText").text = Util.GetText($"{_characterModel.DefensePower}").ToString();

    public void UpdateAttackPower() => GetUI<TMP_Text>("AttackPowerText").text = Util.GetText($"{_characterModel.AttackPower}").ToString();
    public void UpdateAttackSpeed() => GetUI<TMP_Text>("AttackSpeedText").text = Util.GetText($"{_characterModel.AttackSpeed}").ToString();
    public void UpdateCriticalChance() => GetUI<TMP_Text>("CriticalChanceText").text = Util.GetText($"{_characterModel.CriticalChacnce}").ToString();

   // public void UpdateSkillDamage() => GetUI<TMP_Text>("SkillDamage").text = Util.GetText($"{_characterModel.SkillDamage}").ToString();
   // public void UpdateSkillInterval() => GetUI<TMP_Text>("SkillInterval").text = Util.GetText($"{_characterModel.SkillInterval}").ToString();
    #endregion
}
