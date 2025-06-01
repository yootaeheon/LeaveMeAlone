/*using UnityEngine;

public class TestMonsterData : MonoBehaviour
{
    public DataManager Data;

    [Header("")]
    [SerializeField] int CurChapter;
    [SerializeField] int CurStage;

    private void Start() => InitStatusAfter100(CurChapter ,CurStage);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitStatusAfter100(CurChapter ,CurStage);
        }
    }

    /// <summary>
    /// Stage에 맞는 몬스터 정보 초기화
    /// 메서드의 매개변수는 stageNum
    /// </summary>
    /// <param name="stageNum"></param>
    public void InitStatusAfter100(int chapter, int stage, float baseHp = 100f, float baseDamage = 10f)
    {
        float multiplier = 1f + (chapter - 1) * 0.2f + (stage - 1) * 0.05f;

        MaxHp = Mathf.Round(baseHp * multiplier * 10f) / 10f; // 소수 첫째 자리까지
        AttackDamage = Mathf.Round(baseDamage * multiplier * 10f) / 10f;

        CurHp = MaxHp; // 체력 초기화
    }
}
*/