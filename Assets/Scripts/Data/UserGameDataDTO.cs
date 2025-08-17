[System.Serializable]
public class UserGameDataDTO
{
    public CharacterModelDTO CharacterModelDTO;
    public ProgressDataDTO ProgressDataDTO;
    public InventoryDTO InventoryDataDTO;
    public GoldDataDTO GoldDataDTO;

    /// <summary>
    ///  기본 생성자
    ///  기본 생성자는 역직렬화(Load /불러오기)할 때 필요함
    /// </summary>
    public UserGameDataDTO() { }

    /// <summary>
    /// 매개변수 생성자
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
