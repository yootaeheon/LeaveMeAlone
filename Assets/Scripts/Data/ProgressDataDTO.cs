using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressDataDTO : MonoBehaviour
{
    public int Chapter;
    public int Stage;
    public int KillCount;

    public ProgressDataDTO(int chapter, int stage, int killCount)
    {
        this.Chapter = chapter;
        this.Stage = stage;
        this.KillCount = killCount;
    }
}
