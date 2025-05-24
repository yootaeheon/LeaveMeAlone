using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class ProgressSO : ScriptableObject
{
    [SerializeField] int _chapter;
    public int Chapter { get { return _chapter; } set { _chapter = value; } }

    [SerializeField] int _stage;
    public int Stage { get { return _stage; } set { _stage = value; } }

    [SerializeField] int _monsterNumInStage;
    public int MonsterNumInStage { get { return _monsterNumInStage; } set {_monsterNumInStage = value; } }
}
