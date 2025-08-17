using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoldDataDTO
{
    public int Gold;
    public int Gem;
    
    public GoldDataDTO() { }

    public GoldDataDTO(int gold, int gem)
    {
        this.Gold = gold;
        this.Gem = gem;
    }
}
