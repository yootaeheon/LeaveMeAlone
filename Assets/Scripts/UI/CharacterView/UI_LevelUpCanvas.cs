using UnityEngine;

public class UI_LevelUpCanvas : MonoBehaviour
{
    [SerializeField] CharacterModel _model;

    [SerializeField] Transform _player;

    private Vector3 _playerPos => _player.position + new Vector3(0, 0.5f ,0 );

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
    public void Button_MaxHp()
    {
        if (GameManager.Instance.Gold < 1000)
            return;

        GameManager.Instance.Gold -= 1000;

        _model.MaxHp += 0.5f;
        EffectManager.Instance.PlayEffect(EffectManager.Instance.EffectData.MaxHpEffect, _playerPos, _player);
    }

    public void Button_RecoveryHp()
    {
        if (GameManager.Instance.Gold < 2000)
            return;

        GameManager.Instance.Gold -= 2000;

        _model.RecoverHpPerSecond += 0.1f;
        EffectManager.Instance.PlayEffect(EffectManager.Instance.EffectData.RecoveryEffect, _playerPos, _player);
    }

    public void Button_DefensePower()
    {
        if (GameManager.Instance.Gold < 1000)
            return;

        GameManager.Instance.Gold -= 1000;

        _model.DefensePower += 0.1f;
        EffectManager.Instance.PlayEffect(EffectManager.Instance.EffectData.DefensePowerEffect, _playerPos, _player);
    }

    public void Button_AttackPower()
    {
        if (GameManager.Instance.Gold < 1000)
            return;

        GameManager.Instance.Gold -= 1000;

        _model.AttackPower += 0.1f;
        EffectManager.Instance.PlayEffect(EffectManager.Instance.EffectData.AttackPowerEffect, _playerPos, _player);
    }

    public void Button_AttackSpeed()
    {
        if (GameManager.Instance.Gold < 2500)
            return;

        GameManager.Instance.Gold -= 2500;

        _model.AttackSpeed += 0.05f;
        EffectManager.Instance.PlayEffect(EffectManager.Instance.EffectData.AttackSpeedEffect, _playerPos, _player);
    }

    public void Button_CriticalChance()
    {
        if (GameManager.Instance.Gold < 2500)
            return;

        GameManager.Instance.Gold -= 2500;

        _model.CriticalChance += 0.05f;
        EffectManager.Instance.PlayEffect(EffectManager.Instance.EffectData.CriticalChanceEffect, _playerPos, _player);
    }
    #endregion
}
