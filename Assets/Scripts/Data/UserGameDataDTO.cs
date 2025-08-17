[System.Serializable]
public class UserGameDataDTO
{
    public CharacterModelDTO CharacterModelDTO;
    public ProgressDataDTO ProgressDataDTO;
    public InventoryDTO InventoryDataDTO;
    public GoldDataDTO GoldDataDTO;

    /// <summary>
    ///  �⺻ ������
    ///  �⺻ �����ڴ� ������ȭ(Load /�ҷ�����)�� �� �ʿ���
    /// </summary>
    public UserGameDataDTO() { }

    /// <summary>
    /// �Ű����� ������
    /// </summary>
    /// <param name="characterStatusDTO"></param>
    /// <param name="progressDataDTO"></param>
    /// <param name="inventoryDataDTO"></param>
    /// <param name="goldDataDTO"></param>
    public UserGameDataDTO(CharacterModelDTO characterStatusDTO, ProgressDataDTO progressDataDTO, InventoryDTO inventoryDataDTO, GoldDataDTO goldDataDTO)
    {
        this.CharacterModelDTO = characterStatusDTO;
        this.ProgressDataDTO = progressDataDTO;
        this.InventoryDataDTO = inventoryDataDTO;
        this.GoldDataDTO = goldDataDTO;
    }
}
