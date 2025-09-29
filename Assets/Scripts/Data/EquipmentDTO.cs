using Inventory.Model;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EquipmentDTO
{
    public List<ItemDTO> EquippedItems = new();

    // 기본 생성자
    public EquipmentDTO()
    {
        EquippedItems = new List<ItemDTO>();
    }

    // EquipmentManager의 _equippedItems를 기반으로 DTO 생성
    public EquipmentDTO(Dictionary<EquipmentType, EquipItemSO> equippedItems)
    {
        EquippedItems = new List<ItemDTO>();
        foreach (var kvp in equippedItems)
        {
            EquipItemSO item = kvp.Value;
            if (item != null)
            {
                EquippedItems.Add(new ItemDTO
                {
                    ItemIndex = item.ID,
                    Quantity = 1, // 장비 아이템은 일반적으로 수량이 1
                    Item = item
                });
            }
        }
    }
}
