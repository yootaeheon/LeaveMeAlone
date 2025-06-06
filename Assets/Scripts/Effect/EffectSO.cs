using UnityEngine;

[CreateAssetMenu()]
public class EffectSO : ScriptableObject
{
    public GameObject[] AttackEffects;

    [field: SerializeField] public GameObject AttackPowerEffect { get; private set; }
    [field: SerializeField] public GameObject RecoveryEffect { get; private set; }
    [field: SerializeField] public GameObject DefensePowerEffect { get; private set; }
    [field: SerializeField] public GameObject AttackSpeedEffect { get; private set; }
    [field: SerializeField] public GameObject CriticalChanceEffect { get; private set; }
    [field: SerializeField] public GameObject DeadEffect { get; private set; }
    [field: SerializeField] public GameObject MaxHpEffect { get; private set; }
    [field: SerializeField] public GameObject HpItemEffect { get; private set; }
    [field: SerializeField] public GameObject DebuffItemEffect { get; private set; }

    // 속성 공격 이펙트
    [field: SerializeField] public GameObject BloodEffect { get; private set; }
    [field: SerializeField] public GameObject FireEffect { get; private set; }
    [field: SerializeField] public GameObject WaterEffect { get; private set; }
    [field: SerializeField] public GameObject IceEffet { get; private set; }
    [field: SerializeField] public GameObject LightningEffect { get; private set; }
    [field: SerializeField] public GameObject BombEffect { get; private set; }
    [field: SerializeField] public GameObject Dark { get; private set; }
    [field: SerializeField] public GameObject Poison { get; private set; }

    private void OnEnable()
    {
        AttackEffects = new GameObject[8]
        {
              BloodEffect, FireEffect, WaterEffect, IceEffet, 
              LightningEffect, BombEffect, Dark, Poison
        };
    }
}
