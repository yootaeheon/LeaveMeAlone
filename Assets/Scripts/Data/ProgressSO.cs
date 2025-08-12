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
    public event Action OnClearStage; // FadeIn ȿ�� ����
    public event Action OnKillCountChanged;
    public int KillCount
    {
        get { return _killCount; }
        set
        {
           /* if (_killCount == value) return;*/

            _killCount = value;

            if (_killCount == 0)
            {
                OnClearStage?.Invoke();
                _killCount = 5;
            }

            OnKillCountChanged?.Invoke();
        }
    }
}
