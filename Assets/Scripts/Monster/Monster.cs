using System;
using UnityEngine;

[System.Serializable]
public class Monster : MonoBehaviour
{
    // Monster Status 계산식
    // MaxHp = BaseHp × (1 + (Chapter - 1) × 0.2 + (Stage - 1) × 0.05);
    // AttackDamage = BaseDamage × (1 + (Chapter - 1) × 0.2 + (Stage - 1) × 0.05);
    // 챕터가 올라가면 (Chapter - 1) * 0.2만큼, 스테이지가 올라가면 (Stage - 1) * 0.05만큼 난이도가 증가
    public int CurChapter => ChapterManager.Instance.ProgressInfo.Chapter;
    public int CurStage => ChapterManager.Instance.ProgressInfo.Stage;

    public int Progress => (CurChapter-1) * 10 + CurStage;
    private void Start() => InitStatusAfter100(CurChapter, CurStage);


    private void OnEnable()
    {
        ChapterManager.Instance.ProgressInfo.OnStageChanged += () => InitStatusAfter100(CurChapter, CurStage);
        ChapterManager.Instance.ProgressInfo.OnStageChanged += () => InitStatus(Progress);
        InitStatus(Progress);
        InitStatusAfter100(CurChapter, CurStage);
    }

    private void OnDisable()
    {
        ChapterManager.Instance.ProgressInfo.OnStageChanged -= () => InitStatusAfter100(CurChapter, CurStage);
        ChapterManager.Instance.ProgressInfo.OnStageChanged -= () => InitStatus(Progress);
    }

  
    /// <summary>
    /// 10챕터 10스테이지 이후 몬스터 능력치 상승 공식
    /// 확장성과 유지보수성을 고려
    /// </summary>
    /// <param name="chapter"></param>
    /// <param name="stage"></param>
    /// <param name="baseHp"></param>
    /// <param name="baseDamage"></param>
    public void InitStatusAfter100(int chapter, int stage, float baseHp = 100f, float baseDamage = 10f)
    {
        float multiplier = 1f + (chapter - 1) * 0.2f + (stage - 1) * 0.05f;

        MaxHp = Mathf.Round(baseHp * multiplier * 10f) / 10f;               // 소수 첫째 자리까지
        AttackDamage = Mathf.Round(baseDamage * multiplier * 10f) / 10f;

        CurHp = MaxHp; // 체력 초기화
    }

    /// <summary>
    /// Stage에 맞는 몬스터 정보 초기화
    /// 메서드의 매개변수는 stageNum
    /// </summary>
    public void InitStatus(int progress)
    {
        MaxHp = int.Parse(DataManager.Instance.MonsterCSV.GetData(progress, (int)MonsterData.MaxHp));
        AttackDamage = int.Parse(DataManager.Instance.MonsterCSV.GetData(progress, (int)MonsterData.AttackDamage));
    }


    [Header("Monster Status")]
    [SerializeField] bool _isDead;
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }

    [SerializeField] float _curHp;
    public float CurHp { get { return _curHp; } set { _curHp = value; OnCurHpChanged?.Invoke(); } }
    public Action OnCurHpChanged;

    [SerializeField] float _maxHp;
    public float MaxHp { get { return _maxHp; } set { _maxHp = value; } }


    [Header("TempAttack")]
    [SerializeField] LayerMask _enemyLayer;
    public LayerMask EnemyLayer { get { return _enemyLayer; } set { _enemyLayer = value; } }

    [SerializeField] float _attackRange;
    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }

    [SerializeField] float _attackDamage;
    public float AttackDamage { get { return _attackDamage; } set { _attackDamage = value; } }

    [SerializeField] float _attackInterval;
    public float AttackInterval { get { return _attackInterval; } set { _attackInterval = value; } }


    public UI_HealthBar _healthBar;

}
