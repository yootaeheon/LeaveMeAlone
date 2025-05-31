using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EffectSO : ScriptableObject
{
    [field: SerializeField] public GameObject AttackPowerEffect { get; private set; }
    [field: SerializeField] public GameObject RecoveryEffect { get; private set; }
    [field: SerializeField] public GameObject DefensePowerEffect { get; private set; }
    [field: SerializeField] public GameObject AttackSpeedEffect { get; private set; }
    [field: SerializeField] public GameObject CriticalChanceEffect { get; private set; }
    [field: SerializeField] public GameObject DeadEffect { get; private set; }
    [field: SerializeField] public GameObject MaxHpEffect { get; private set; }
    [field: SerializeField] public GameObject HpItemEffect { get; private set; }
    [field: SerializeField] public GameObject DebuffItemEffect { get; private set; }
}
