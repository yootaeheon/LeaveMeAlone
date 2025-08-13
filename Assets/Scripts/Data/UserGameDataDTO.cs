[System.Serializable]
public class UserGameDataDTO
{
    public CharacterModelDTO CharacterModelDTO;
    public ProgressDataDTO ProgressDataDTO;
    public InventoryDTO InventoryDataDTO;

    /// <summary>
    ///  기본 생성자
    /// </summary>
    public UserGameDataDTO() { }

    /// <summary>
    /// 매개변수 생성자
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
