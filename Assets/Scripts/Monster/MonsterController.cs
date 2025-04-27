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

            yield return Util.GetDelay(_model.AttackInterval);

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
        _model.CurHp -= Damage;
        Debug.Log($"{Damage}만큼 피해 입음");

        transform.DOShakePosition(0.3f, 0.2f);

        _spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(()=> _spriteRenderer.DOColor(_originColor, 0.1f));

        if (_model.CurHp <= 0)
        {
            CurHp = 0;
            Die();
        }
    }

    void Die()
    {
        // DoTween을 사용하여 몬스터가 사라지는 애니메이션
        Sequence deathSequence = DOTween.Sequence();

        // 페이드아웃(투명화)
        deathSequence.Append(_spriteRenderer.DOFade(0, 0.5f));

        // 크기 축소
        deathSequence.Join(transform.DOScale(Vector3.zero, 0.5f));

        // 애니메이션 완료 후 오브젝트 삭제
        deathSequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
