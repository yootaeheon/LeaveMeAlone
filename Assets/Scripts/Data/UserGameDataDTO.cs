[System.Serializable]
public class UserGameDataDTO
{
    public CharacterModelDTO CharacterModelDTO;
    public ProgressDataDTO ProgressDataDTO;
    public InventoryDTO InventoryDataDTO;

    /// <summary>
    ///  �⺻ ������
    /// </summary>
    public UserGameDataDTO() { }

    /// <summary>
    /// �Ű����� ������
    /// </summary>
    /// <param name="characterStatusDTO"></param>
    /// <param name="progressDataDTO"></param>
    /// <param name="inventoryDataDTO"></param>
    public UserGameDataDTO(CharacterModelDTO characterStatusDTO, ProgressDataDTO progressDataDTO, InventoryDTO inventoryDataDTO)
    {
        this.CharacterModelDTO = characterStatusDTO;
        this.ProgressDataDTO = progressDataDTO;
        this.InventoryDataDTO = inventoryDataDTO;
    }
}
