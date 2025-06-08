[System.Serializable]
public class CharacterModelDTO
{
    public float MaxHp;
    public float RecoverHpPerSecond;
    public float DefensePower;

    public float AttackPower;
    public float AttackSpeed;
    public float CriticalChance;

    // 데이터를 JSON으로부터 객체로 역직렬화할 때, 기본 생성자가 없으면 내부적으로 객체를 생성할 수 없기 때문에 기본 생성자를 만듬
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
