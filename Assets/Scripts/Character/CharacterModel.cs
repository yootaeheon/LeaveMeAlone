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


    [Header("Attack")]
    [SerializeField] LayerMask _enemyLayer;
    public LayerMask EnemyLayer { get { return _enemyLayer; } set { _enemyLayer = value; } }

    [SerializeField] float _attackRange;
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }

    [SerializeField] float _attackDamage;
    public float AttackDamage { get { return _attackDamage; } set { _attackDamage = value; } }

    [SerializeField] float _attackInterval;
    public float AttackInterval { get { return _attackInterval; } set { _attackInterval = value; } }


    [Header("Skill")]
    [SerializeField] float _skillDamage;
    public float SkillDamage { get { return _skillDamage; } set { _skillDamage = value; } }

    [SerializeField] float _skillInterval;
    public float SkillInterval { get { return _skillInterval; } set { _skillInterval = value; } }

  /*  [Header("Equip")]
    [SerializeField] EquippableItemSO */

    private void Start()
    {
        CurHp = MaxHp; // 초기 HP 설정
    }
}
