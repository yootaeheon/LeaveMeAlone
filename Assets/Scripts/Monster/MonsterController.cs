using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public enum MonsterState { Idle, Move, Detect, Attack }

public class MonsterController : Monster, IDamageable
{
    [Header("")]
    public Monster _model;

    private Transform character;

    private SpriteRenderer _spriteRenderer;

    private PooledObject _pooledObject;

    private Color _originColor;

    private MonsterState currentState = MonsterState.Move;

    private CharacterController _characterController;

    private MonsterSpawner _spawner;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
        _pooledObject = GetComponent<PooledObject>();
        _characterController = FindAnyObjectByType<CharacterController>();
        _spawner = FindObjectOfType<MonsterSpawner>();
    }

    private void OnEnable()
    {
        IsDead = false;
        CurHp = MaxHp;
        currentState = MonsterState.Move;
    }

    private void OnDisable()
    {
        IsDead = true;
    }

    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                SearchForEnemies();
                break;

            case MonsterState.Move:
                Move();
                SearchForEnemies();
                break;

            case MonsterState.Detect:
                if (character != null)
                    StartCoroutine(AttackRoutine());
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

    IEnumerator AttackRoutine()
    {
        currentState = MonsterState.Attack;

        while (character != null)
        {
            Attack();

            yield return Util.GetDelay(_model.AttackInterval);

            SearchForEnemies(); // 공격 후 다시 적 탐색
        }

        currentState = MonsterState.Move;
    }

    public void Attack()
    {
        character.GetComponent<IDamageable>().TakeDamage(_model.AttackDamage);
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

        // 애니메이션 완료 후 비활성화 후 재초기화
        deathSequence.OnComplete(() =>
        {
            _pooledObject.CallReturnPool();
            _characterController._monster = null;
            transform.localScale = Vector3.one;
            _spriteRenderer.DOFade(1, 0f);
            _spawner.Spawn();
        });
    }
}
