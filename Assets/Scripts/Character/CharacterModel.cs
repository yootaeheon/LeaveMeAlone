using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterModel : MonoBehaviour
{
    [Header("Character Status")]
    [SerializeField] float _curHp;
    public float CurHp { get { return _curHp; } set { _curHp = value; } }

    [SerializeField] float _maxHp;
    public float MaxHp { get { return _maxHp; } set { _maxHp = value; } }

    [SerializeField] float _recoverHpPerSecond;
    public float RerecoverHpPerSecond { get { return RerecoverHpPerSecond; } set { RerecoverHpPerSecond = value; } }

    [SerializeField] float _defensePower;
    public float DefensePower { get { return _defensePower; } set { _defensePower = value; } }



    [Header("Attack")]
    [SerializeField] LayerMask _enemyLayer = 1 << 6;
    public LayerMask EnemyLayer { get { return _enemyLayer; } set { _enemyLayer = value; } }

    [SerializeField] float _attackRange;
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }

    [SerializeField] float _attackPower;
    public float AttackPower { get { return _attackPower; } set { _attackPower = value; } }

    [SerializeField] float _attackSpeed;
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    [SerializeField] float _criticalChance;
    public float CriticalChacnce {  get { return _criticalChance; } set { _criticalChance = value; } }


    [Header("Skill")]
    [SerializeField] float _skillDamage;
    public float SkillDamage { get { return _skillDamage; } set { _skillDamage = value; } }

    [SerializeField] float _skillInterval;
    public float SkillInterval { get { return _skillInterval; } set { _skillInterval = value; } }

  /*  [Header("Equip")]
    [SerializeField] EquippableItemSO */

    private void Awake()
    {
        CurHp = MaxHp; // 초기 HP 설정
    }
}
