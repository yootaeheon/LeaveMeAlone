using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using System;
using Unity.Mathematics;

public enum CharacterState { Idle, Move, Detect, Attack }

public class CharacterController : MonoBehaviour, IDamageable
{
    public CharacterModel _model;

    [HideInInspector] public Transform _monster;

    private CharacterState _currentState = CharacterState.Move;

    private Animator _animator;

    public Action OnEncounterMonster { get; set; }       // 몬스터 조우 시, 발생하는 이벤트 (맵 스크롤링 정지)
    public Action OnKillMonster { get; set; }                 // 몬스터 처치 시, 발생하는 이벤트 (맵 스크롤링 진행)

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        recoveryHpRoutine = StartCoroutine(RecoveryHpRoutine());
    }

    void Update()
    {
        switch (_currentState)
        {
            case CharacterState.Idle:
                _animator.SetBool("1_Move", false);
                SearchForEnemies();
                break;

            case CharacterState.Move:
                _animator.SetBool("1_Move", true);
                SearchForEnemies();
                break;

            case CharacterState.Detect:
                if (_monster != null)
                {
                    _animator.SetBool("1_Move", false);
                    StartCoroutine(AttackRoutine());
                }
                break;
        }
    }

    void SearchForEnemies()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, _model.AttackRange, _model.EnemyLayer);
        if (enemy != null)
        {
            if (enemy.GetComponent<Monster>().IsDead)
            {
                return;
            }
            _monster = enemy.transform;
            _currentState = CharacterState.Detect;
            OnEncounterMonster?.Invoke();
        }
    }

    IEnumerator AttackRoutine()
    {
        _currentState = CharacterState.Attack;

        while (_monster != null)
        {
            _animator.SetTrigger("2_Attack");

            yield return new WaitForSeconds(_model.AttackSpeed);

            SearchForEnemies(); // 공격 후 다시 적 탐색
        }

        _currentState = CharacterState.Move;
        OnKillMonster?.Invoke();
    }

    public void Attack()
    {
        _monster.GetComponent<IDamageable>().TakeDamage(_model.AttackPower);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _model.AttackRange); // 공격 범위 표시
    }

    public void TakeDamage(float Damage)
    {
        _model.CurHp -= Damage;

        transform.DOShakePosition(0.2f, 0.1f);
    }

    Coroutine recoveryHpRoutine;
    private IEnumerator RecoveryHpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (_model.CurHp <= 0) 
                yield break;

            _model.CurHp += _model.RerecoverHpPerSecond;

            if (_model.CurHp > _model.MaxHp)
            {
                _model.CurHp = _model.MaxHp;
            }
        }
    }
}
