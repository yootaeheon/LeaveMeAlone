using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

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

    public Action OnEncounterMonster { get; set; }       // ���� ���� ��, �߻��ϴ� �̺�Ʈ (�� ��ũ�Ѹ� ����)
    public Action OnKillMonster { get; set; }            // ���� óġ ��, �߻��ϴ� �̺�Ʈ (�� ��ũ�Ѹ� ����)
    public Action OnSettedInit { get; set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        DatabaseManager.Instance.OnGameDataLoaded += () =>
        {
            Subscribe();
            recoveryHpRoutine = StartCoroutine(RecoveryHpRoutine());
            OnSettedInit?.Invoke();
        };
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    public void Init()
    {
        /*SetFromDTO(DatabaseManager.Instance.GameData.CharacterModelDTO);*/
        Subscribe();
        Debug.Log("���� �Ϸ�");
        recoveryHpRoutine = StartCoroutine(RecoveryHpRoutine());
        Debug.Log("ü��ȸ�� �ڷ�ƾ ���� ");
        OnSettedInit?.Invoke();
        Debug.Log("��� �ʱ�ȭ �Ϸ� �� ���� ���� �̺�Ʈ ȣ��!");
    }

    #region Subscribe/Unsubscribe
    public void Subscribe()
    {
        Model.MaxHpChanged += View.UpdateMaxHp;
        Model.CurHpChanged += View.UpdateCurHp;
        Model.CurHpChanged += () => _healthBar.UpdateHealthBar(Model.CurHp, Model.MaxHp);
        Model.RecoverHpPerSecondChanged += View.UpdateRerecoverHpPerSecond;
        Model.DefensePowerChanged += View.UpdateDefensePower;
        Model.AttackPowerChanged += View.UpdateAttackPower;
        Model.AttackSpeedChanged += View.UpdateAttackSpeed;
        Model.CriticalChanceChanged += View.UpdateCriticalChance;
        //Model.SkillDamageChanged += View
        //Model.SkillIntervalChanged +=View
    }

    public void UnSubscribe()
    {
        Model.CurHpChanged -= View.UpdateCurHp;
        Model.CurHpChanged -= () => _healthBar.UpdateHealthBar(Model.CurHp, Model.MaxHp);
        Model.MaxHpChanged -= View.UpdateMaxHp;
        Model.RecoverHpPerSecondChanged -= View.UpdateRerecoverHpPerSecond;
        Model.DefensePowerChanged -= View.UpdateDefensePower;
        Model.AttackPowerChanged -= View.UpdateAttackPower;
        Model.AttackSpeedChanged -= View.UpdateAttackSpeed;
        Model.CriticalChanceChanged -= View.UpdateCriticalChance;
        //Model.SkillDamageChanged -= View
        //Model.SkillIntervalChanged -=View
    }
    #endregion

    /// <summary>
    /// FSM �����Ͽ� ���� �� �ൿ ����
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

    public void SetFromDTO(CharacterModelDTO dto)
    {
        Debug.Log("DTO �ʱ�ȭ ��");
        Model.MaxHp = dto.MaxHp;
        Model.RecoverHpPerSecond = dto.RecoverHpPerSecond;
        Model.DefensePower = dto.DefensePower;
        Model.AttackPower = dto.AttackPower;
        Model.AttackSpeed = dto.AttackSpeed;
        Model.CriticalChance = dto.CriticalChance;
        Debug.Log("DTO �ʱ�ȭ ��");

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

            SearchForEnemies(); // ���� �� �ٽ� �� Ž��
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
        Gizmos.DrawWireSphere(transform.position, Model.AttackRange); // ���� ���� ǥ��
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
        seq.Append(_damageText.rectTransform.DOAnchorPosY(100f, 1f)) // anchoredPosition ����
           .Join(_damageText.DOFade(0, 1f))
           .OnComplete(() =>
           {
               _damageText.alpha = 0;
               _damageText.rectTransform.anchoredPosition = Vector2.zero;
           });
    }

    public IEnumerator Die()
    {
        /*_monster = null; // ��� ��*/
        _animator.Play("DEATH", 0, 0f);
        EffectManager.Instance.PlayEffect(EffectManager.Instance.EffectData.DeadEffect, transform.position);
        yield return Util.GetDelay(1F);
        CameraUtil.CameraFadeIn();
        ChapterManager.Instance.ProgressInfo.KillCount = 5;
        /*_objectPool.ResetPos(); // ���� ��ġ �ʱ�ȭ �� ���� ��ġ�� ������ �ָ��� �����ϴ� ���� �߻���.. �ּ� ó���س��� */

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

            Model.CurHp += Model.RecoverHpPerSecond;

            if (Model.CurHp > Model.MaxHp)
            {
                Model.CurHp = Model.MaxHp;
            }
        }
    }
}