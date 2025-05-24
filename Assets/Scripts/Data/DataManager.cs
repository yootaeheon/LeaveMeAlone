using CustomUtility.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance {  get; private set; }
    [field : SerializeField] public CsvTable MonsterCSV { get; private set; }

    private void Awake() => Init();

    /*private void Start() => TestMethod();*/

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        CsvReader.Read(MonsterCSV);
    }

    private void TestMethod()
    {
        /* // 2 Stage의 몬스터들의 MaxHp 불러오기
        Debug.Log(MonsterCSV.GetData(2, (int)MonsterData.MaxHp));*/
    }
}

public enum MonsterData
{
    Num = 1,
    MaxHp,
    AttackDamage,
}