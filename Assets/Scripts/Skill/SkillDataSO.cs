using UnityEngine;

public enum SkillTarget
{
    Ally,
    Enemy,
    RandomEnemy,
    BoosEnemy
}

public enum SkillType
{
    Attack,
    Defense,
    control
}

public enum SkillTypeDetail
{
    // 공격형 효과
    AoE,
    SingleTarget,
    DoT,
    Chain,

    // 방어형 효과
    ShieldAll,
    HealAll,

    // 제어형 효과
    Stun,
    Bind,
    ForceMove,
    Frenzy
}

public class SkillDataSO : ScriptableObject
{
    [SerializeField] SkillTarget _target;
    public SkillTarget Target { get { return _target; } }

    [SerializeField] SkillType _skillType;
    public SkillType SkillType { get { return _skillType; } }

    [SerializeField] AudioClip _skillEffect;
    public AudioClip SkillEffect { get { return _skillEffect; } }

    [SerializeField] float _coolTime;
    public float CoolTime { get { return _coolTime; } }

    [SerializeField] float _value;
    public float Value { get { return _value; } }

    [SerializeField] float _damageDuration;
    public float DamageDuration { get { return _damageDuration; } }
}
