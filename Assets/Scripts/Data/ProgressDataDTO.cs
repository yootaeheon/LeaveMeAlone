using System.Collections;
[System.Serializable]
public class ProgressDataDTO
{
    public int Chapter;
    public int Stage;
    public int KillCount;

    public ProgressDataDTO() { }

    public ProgressDataDTO(int chapter, int stage, int killCount)
    {
        this.Chapter = chapter;
        this.Stage = stage;
        this.KillCount = killCount;
    }
}
