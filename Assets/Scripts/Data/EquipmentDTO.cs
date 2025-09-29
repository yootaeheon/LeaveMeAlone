using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

// 현재 장착중인 아이템 정보를 저장하는 DTO
[Serializable]
public class EquipmentSlot
{
    public EquipmentType Type;
    public int ItemID;
}

[Serializable]
public class EquipmentDTO
{
    public List<EquipmentSlot> EquippedItems = new();

    // 기본 생성자
    public EquipmentDTO() { }

    // EquipmentManager의 _equippedItems를 기반으로 DTO 생성
    public EquipmentDTO(Dictionary<EquipmentType, EquipItemSO> equippedItems)
    {
        EquippedItems = new List<EquipmentSlot>();
        foreach (var kvp in equippedItems)
        {
            EquippedItems.Add(new EquipmentSlot
            {
                Type = kvp.Key,
                ItemID = kvp.Value != null ? kvp.Value.ID : 0
            });
        }
    }
}
