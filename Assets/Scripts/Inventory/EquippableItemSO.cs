using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public class EquippableItemSO : ItemSO, IItemAction
    {
        public string ActionName => "Equip";

        public AudioClip ActionSFX { get; private set; }    
        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}