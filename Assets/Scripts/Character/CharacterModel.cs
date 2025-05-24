using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterModel : MonoBehaviour
{
    [Header("Character Status")]
    [SerializeField] float _curHp;
    public float CurHp { get { return _curHp; } set { _curHp = value; CurHpChanged?.Invoke(); } }
    public Action CurHpChanged;

    [SerializeField] float _maxHp;
    public float MaxHp { get { return _maxHp; } set { _maxHp = value; MaxHpChanged?.Invoke(); } }
    public Action MaxHpChanged;

    [SerializeField] float _recoverHpPerSecond;
    public float RerecoverHpPerSecond { get { return _recoverHpPerSecond; } set { _recoverHpPerSecond = value; RerecoverHpPerSecondChanged?.Invoke(); } }
    public Action RerecoverHpPerSecondChanged { get; set; }

    [SerializeField] float _defensePower;
    public float DefensePower { get { return _defensePower; } set { _defensePower = value; DefensePowerChanged?.Invoke(); } }
    public Action DefensePowerChanged;




    [Header("TempAttack")]
    [SerializeField] LayerMask _enemyLayer = 1 << 6;
    public LayerMask EnemyLayer { get { return _enemyLayer; } set { _enemyLayer = value; } }

    [SerializeField] float _attackRange;
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }

    [SerializeField] float _attackPower;
    public float AttackPower { get { return _attackPower; } set { _attackPower = value; AttackPowerChanged?.Invoke(); } }
    public Action AttackPowerChanged;

    [SerializeField] float _attackSpeed;
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; AttackSpeedChanged?.Invoke(); } }
    public Action AttackSpeedChanged;

    [SerializeField] float _criticalChance;
    public float CriticalChacnce {  get { return _criticalChance; } set { _criticalChance = value; CriticalChacnceChanged?.Invoke(); } }
    public Action CriticalChacnceChanged;


    [Header("Skill")]
    [SerializeField] float _skillDamage;
    public float SkillDamage { get { return _skillDamage; } set { _skillDamage = value; SkillDamageChanged?.Invoke(); } }
    public Action SkillDamageChanged;

    [SerializeField] float _skillInterval;
    public float SkillInterval { get { return _skillInterval; } set { _skillInterval = value; SkillIntervalChanged?.Invoke(); } }
    public Action SkillIntervalChanged;

  /*  [Header("Equip")]
    [SerializeField] EquippableItemSO */




    private void Awake()
    {
        CurHp = MaxHp; // 초기 HP 설정
    }
}
