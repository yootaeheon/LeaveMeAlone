using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private Dictionary<EquipmentType, EquipItemSO> equippedItems = new();

    public void EquipItem(EquipItemSO item)
    {
        equippedItems[item.EquipmentType] = item;
        // 스탯 반영 로직 등...
    }

    public void UnequipItem(EquipmentType type)
    {
        if (equippedItems.ContainsKey(type))
            equippedItems.Remove(type);
    }

    public EquipItemSO GetEquippedItem(EquipmentType type)
    {
        equippedItems.TryGetValue(type, out EquipItemSO item);
        return item;
    }
}
