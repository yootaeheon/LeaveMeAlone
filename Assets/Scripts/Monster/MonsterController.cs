using System.Collections;
using UnityEngine;
using DG.Tweening;

public enum MonsterState { Idle, Move, Detect, Attack }

public class MonsterController : Monster, IDamageable
{
    [Header("")]
    public Monster _model;

    private Transform character;

    private SpriteRenderer _spriteRenderer;

    private Color _originColor;

    private MonsterState currentState = MonsterState.Idle;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
    }

    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                Move();
                SearchForEnemies();
                break;

            case MonsterState.Move:
                Move();
                SearchForEnemies();
                break;

            case MonsterState.Detect:
                if (character != null)
                    StartCoroutine(AttackEnemy());
                break;
        }
    }

    void SearchForEnemies()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, _model.AttackRange, _model.EnemyLayer);
        if (enemy != null)
        {
            character = enemy.transform;
            currentState = MonsterState.Detect;
        }
    }

    void Move()
    {
        // 단순 이동 로직 (예: 플레이어 입력, AI 이동 가능)
        transform.Translate(Vector2.left * _model.MoveSpeed * Time.deltaTime);
    }

    IEnumerator AttackEnemy()
    {
        currentState = MonsterState.Attack;

        while (character != null)
        {
            character.GetComponent<IDamageable>().TakeDamage(_model.AttackDamage);

            yield return new WaitForSeconds(_model.AttackInterval);

            SearchForEnemies(); // 공격 후 다시 적 탐색
        }

        currentState = MonsterState.Idle;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _model.AttackRange); // 공격 범위 표시
    }

    public void TakeDamage(float Damage)
    {
        Debug.Log($"{Damage}의 피해를 캐릭터에게 주었다!");
        transform.DOShakePosition(0.3f, 0.2f);
        _spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(()=> _spriteRenderer.DOColor(_originColor, 0.1f));

    }
}
