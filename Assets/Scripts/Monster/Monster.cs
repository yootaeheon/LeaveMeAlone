using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster : MonoBehaviour
{
    [Header("Monster Status")]
    [SerializeField] bool _isDead;
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }

    [SerializeField] float _curHp;
    public float CurHp { get { return _curHp; } set { _curHp = value; OnCurHpChanged?.Invoke(); } }
    public Action OnCurHpChanged;

    [SerializeField] float _maxHp;
    public float MaxHp { get { return _maxHp; } set { _maxHp = value; } }

    [SerializeField] float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }


    [Header("Attack")]
    [SerializeField] LayerMask _enemyLayer;
    public LayerMask EnemyLayer { get { return _enemyLayer; } set { _enemyLayer = value; } }

    [SerializeField] float _attackRange;
    public float AttackRange { get { return _attackRange; }  set { _attackRange = value; } }

    [SerializeField] float _attackDamage;
    public float AttackDamage { get { return _attackDamage; } set { _attackDamage = value; } }

    [SerializeField] float _attackInterval;
    public float AttackInterval { get { return _attackInterval; } set { _attackInterval = value; } }

    public UI_HealthBar _healthBar;

}
