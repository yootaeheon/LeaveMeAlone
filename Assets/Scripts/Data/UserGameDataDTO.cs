[System.Serializable]
public class UserGameDataDTO
{
    public CharacterModelDTO CharacterModelDTO;
    public ProgressDataDTO ProgressDataDTO;

    public UserGameDataDTO() { }

    public UserGameDataDTO(CharacterModelDTO characterStatusDTO, ProgressDataDTO progressDataDTO)
    {
        this.CharacterModelDTO = characterStatusDTO;
        this.ProgressDataDTO = progressDataDTO;
    }
}
