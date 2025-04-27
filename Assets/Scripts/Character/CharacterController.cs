using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using System;

public enum CharacterState { Idle, Move, Detect, Attack }

public class CharacterController : MonoBehaviour, IDamageable
{
    public CharacterModel _model;

    private Transform monster;

    private CharacterState currentState = CharacterState.Idle;

    public Action OnEncounterMonster;                                    // 몬스터 조우 시, 발생하는 이벤트 (맵 스크롤링 정지)
    public Action OnKillMonster;                                         // 몬스터 처치 시, 발생하는 이벤트 (맵 스크롤링 진행)


    void Update()
    {
        switch (currentState)
        {
            case CharacterState.Idle:
                SearchForEnemies();
                break;

            case CharacterState.Move:
                Move();
                SearchForEnemies();
                break;

            case CharacterState.Detect:
                if (monster != null)
                    StartCoroutine(AttackEnemy());
                break;
        }
    }

    void SearchForEnemies()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, _model.AttackRange, _model.EnemyLayer);
        if (enemy != null)
        {
            monster = enemy.transform;
            currentState = CharacterState.Detect;
            OnEncounterMonster?.Invoke();
        }
    }

    void Move()
    {
        
    }

    IEnumerator AttackEnemy()
    {
        currentState = CharacterState.Attack;

        while (monster != null)
        {
            monster.GetComponent<IDamageable>().TakeDamage(_model.AttackDamage);

            yield return new WaitForSeconds(_model.AttackInterval);

            SearchForEnemies(); // 공격 후 다시 적 탐색
        }

        currentState = CharacterState.Idle;
        OnKillMonster?.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _model.AttackRange); // 공격 범위 표시
    }

    public void TakeDamage(float Damage)
    {
        _model.CurHp -= Damage;

        transform.DOShakePosition(0.3f, 0.2f);
    }
}
