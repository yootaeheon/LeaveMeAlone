using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelUpCanvas : MonoBehaviour
{
    [SerializeField] CharacterModel _model;

    /// <summary>
    /// LevelUp Canvas UI_Progress 활성화
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// LevelUp Canvas UI_Progress 비활성화
    /// </summary>
    public void Hide()
    {
        gameObject?.SetActive(false);
    }



    #region Button Methods
    public void Button_MaxHp() => _model.MaxHp += 0.5f;
    public void Button_RecoveryHp() => _model.RerecoverHpPerSecond += 0.1f;
    public void Button_DefensePower() => _model.DefensePower += 0.1f;
    public void Button_AttackPower() => _model.AttackPower += 0.1f;
    public void Button_AttackSpeed() => _model.AttackSpeed += 0.05f;
    public void Button_CriticalChance() => _model.CriticalChacnce += 0.05f;
    #endregion
}
