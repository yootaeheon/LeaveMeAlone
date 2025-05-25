/*using UnityEngine;

public class TestMonsterData : MonoBehaviour
{
    public DataManager Data;

    [Header("")]
    [SerializeField] int _curChapter;
    [SerializeField] int _curStage;

    private void Start() => InitStatus(_curChapter ,_curStage);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitStatus(_curChapter ,_curStage);
        }
    }

    /// <summary>
    /// Stage에 맞는 몬스터 정보 초기화
    /// 메서드의 매개변수는 stageNum
    /// </summary>
    /// <param name="stageNum"></param>
    public void InitStatus(int chapter, int stage, float baseHp = 100f, float baseDamage = 10f)
    {
        float multiplier = 1f + (chapter - 1) * 0.2f + (stage - 1) * 0.05f;

        MaxHp = Mathf.Round(baseHp * multiplier * 10f) / 10f; // 소수 첫째 자리까지
        AttackDamage = Mathf.Round(baseDamage * multiplier * 10f) / 10f;

        CurHp = MaxHp; // 체력 초기화
    }
}
*/