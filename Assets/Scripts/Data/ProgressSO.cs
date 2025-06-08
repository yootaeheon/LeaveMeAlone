using System;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class ProgressSO : ScriptableObject
{
     public int Progress => ((1 - Chapter) * 10) + Stage;

    [SerializeField] int _chapter;
    public int Chapter { get { return _chapter; } set { _chapter = value; OnChapterChanged?.Invoke(); } }
    public event Action OnChapterChanged;

    [SerializeField] int _stage;
    public int Stage { get { return _stage; } set { _stage = value; OnStageChanged?.Invoke(); } }
    public event Action OnStageChanged;

    [SerializeField] int _killCount;
    public event Action OnClearStage; // FadeIn 효과 연결
    public int KillCount
    {
        get { return _killCount; }
        set
        {
            _killCount = value;
            if (_killCount == 0)
            {
                AllMonsterSpawned();
            }
        }
    }

    private void OnEnable()
    {
        if (DatabaseManager.Instance != null &&
       DatabaseManager.Instance.GameData != null &&
       DatabaseManager.Instance.GameData.ProgressDataDTO != null)
        {
            SetFromDTO(DatabaseManager.Instance.GameData.ProgressDataDTO);
        }
    }

    public void AllMonsterSpawned()
    {
        OnClearStage?.Invoke();
        KillCount = 5;
    }

    public void SetFromDTO(ProgressDataDTO dto)
    {
        Chapter = dto.Chapter;
        Stage = dto.Stage;
        KillCount = dto.KillCount;
    }
}
