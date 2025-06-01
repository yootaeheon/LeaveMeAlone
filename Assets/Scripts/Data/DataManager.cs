using CustomUtility.IO;
using UnityEngine;

/// <summary>
/// CSV Data를 읽고 저장해놓는 클래스
/// CSV 데이터를 읽을 필요가 있을 시 DataManager에서 MonsterCSV를 불러와서 GetData()호출하면 됨
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager Instance {  get; private set; }
    [field : SerializeField] public CsvTable MonsterCSV { get; private set; }

    private void Awake() => Init();
    private void Init()
    {
        SetSingleton();
        CsvReader.Read(MonsterCSV);
    }

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
}

public enum MonsterData
{
    Chapter_Stage,
    Chapter,
    Stage,
    MaxHp,
    AttackDamage,
}