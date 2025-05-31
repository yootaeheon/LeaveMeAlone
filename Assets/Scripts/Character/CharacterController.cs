using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public enum CharacterState { Idle, Move, Detect, Attack }

public class CharacterController : MonoBehaviour, IDamageable
{
    [Header("Model")]
    public CharacterModel Model;

    [Header("View")]
    public CharcaterView View;
    [SerializeField] UI_HealthBar _healthBar;
    [SerializeField] TMP_Text _damageText;

    [Header("Presenter")]
    [HideInInspector] public Transform _monster;

    [SerializeField] ObjectPool _objectPool;

    private IDamageable _monsterDamageable;
    public IDamageable MonsterDamageable => _monsterDamageable ??= _monster.GetComponent<IDamageable>();

    private CharacterState _currentState = CharacterState.Move;

    private Animator _animator;

    public Action OnEncounterMonster { get; set; }       // 몬스터 조우 시, 발생하는 이벤트 (맵 스크롤링 정지)
    public Action OnKillMonster { get; set; }            // 몬스터 처치 시, 발생하는 이벤트 (맵 스크롤링 진행)

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Init();
    }

    public void Init()
    {
       Model.CurHp = Model.MaxHp;
    }

    #region Subscribe/Unsubscribe
    public void Subscribe()
    {
        Model.CurHpChanged += View.UpdateCurHp;
        Model.CurHpChanged += () => _healthBar.UpdateHealthBar(Model.CurHp, Model.MaxHp);
        Model.MaxHpChanged += View.UpdateMaxHp;
        Model.RerecoverHpPerSecondChanged += View.UpdateRerecoverHpPerSecond;
        Model.DefensePowerChanged += View.UpdateDefensePower;
        Model.AttackPowerChanged += View.UpdateAttackPower;
        Model.AttackSpeedChanged += View.UpdateAttackSpeed;
        Model.CriticalChacnceChanged += View.UpdateCriticalChance;
        //Model.SkillDamageChanged += View
        //Model.SkillIntervalChanged +=View
    }

    public void UnSubscribe()
    {
        Model.CurHpChanged -= View.UpdateCurHp;
        Model.CurHpChanged -= () => _healthBar.UpdateHealthBar(Model.CurHp, Model.MaxHp);
        Model.MaxHpChanged -= View.UpdateMaxHp;
        Model.RerecoverHpPerSecondChanged -= View.UpdateRerecoverHpPerSecond;
        Model.DefensePowerChanged -= View.UpdateDefensePower;
        Model.AttackPowerChanged -= View.UpdateAttackPower;
        Model.AttackSpeedChanged -= View.UpdateAttackSpeed;
        Model.CriticalChacnceChanged -= View.UpdateCriticalChance;
        //Model.SkillDamageChanged -= View
        //Model.SkillIntervalChanged -=View
    }
    #endregion

    private void Start()
    {
        Subscribe();
     
        recoveryHpRoutine = StartCoroutine(RecoveryHpRoutine());
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    /// <summary>
    /// FSM 적용하여 상태 별 행동 실행
    /// </summary>
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
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, Model.AttackRange, Model.EnemyLayer);
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

            yield return Util.GetDelay(Model.AttackSpeed);

            if (_monster != null)
                continue;

            SearchForEnemies(); // 공격 후 다시 적 탐색
        }

        _currentState = CharacterState.Move;
        OnKillMonster?.Invoke();
    }

    public void Attack()
    {
        MonsterDamageable.TakeDamage(Model.AttackPower);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Model.AttackRange); // 공격 범위 표시
    }

    public void TakeDamage(float Damage)
    {
        Model.CurHp -= Damage;
        ShowDamageText(Damage);


        transform.DOShakePosition(0.2f, 0.1f);

        if (Model.CurHp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private void ShowDamageText(float damage)
    {
        _damageText.text = damage.ToString();
        _damageText.alpha = 1;
        _damageText.rectTransform.anchoredPosition = Vector2.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(_damageText.rectTransform.DOAnchorPosY(100f, 1f)) // anchoredPosition 기준
           .Join(_damageText.DOFade(0, 1f))
           .OnComplete(() =>
           {
               _damageText.alpha = 0;
               _damageText.rectTransform.anchoredPosition = Vector2.zero;
           });
    }

    public IEnumerator Die()
    {
        /*_monster = null; // 사망 시*/
        _animator.Play("DEATH", 0, 0f);
        EffectManager.Instance.PlayEffect(EffectManager.Instance.EffectData.DeadEffect, transform.position);
        yield return Util.GetDelay(1F);
        CameraUtil.CameraFadeIn();
        ChapterManager.Instance.ProgressInfo.KillCount = 5;
        /*_objectPool.ResetPos(); // 몬스터 위치 초기화 시 서로 위치가 멀지만 멀리서 공부하는 버그 발생함.. 주석 처리해놓음 */

        Init();
    }

    Coroutine recoveryHpRoutine;
    private IEnumerator RecoveryHpRoutine()
    {
        while (true)
        {
            yield return Util.GetDelay(1f);

            if (Model.CurHp <= 0) 
                yield break;

            Model.CurHp += Model.RerecoverHpPerSecond;

            if (Model.CurHp > Model.MaxHp)
            {
                Model.CurHp = Model.MaxHp;
            }
        }
    }
}
