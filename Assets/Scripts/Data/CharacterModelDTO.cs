[System.Serializable]
public class CharacterModelDTO
{
    public float MaxHp;
    public float RecoverHpPerSecond;
    public float DefensePower;

    public float AttackPower;
    public float AttackSpeed;
    public float CriticalChance;

    // 생성자 (선택 사항)
    public CharacterModelDTO() { }

    public CharacterModelDTO(
        float maxHp,
        float recoverHpPerSecond,
        float defensePower,
        float attackPower,
        float attackSpeed,
        float criticalChance)
    {
        this.MaxHp = maxHp;
        this.RecoverHpPerSecond = recoverHpPerSecond;
        this.DefensePower = defensePower;
        this.AttackPower = attackPower;
        this.AttackSpeed = attackSpeed;
        this.CriticalChance = criticalChance;
    }
}
