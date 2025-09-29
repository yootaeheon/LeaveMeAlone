using Inventory.Model;
using System;
using System.Collections.Generic;

[Serializable]
public class InventoryDTO
{
    // 슬롯별 아이템 정보
    public List<ItemDTO> Items;

    // 장착중인 장비 정보
    public List<ItemDTO> EquipItems;

    // 기본 생성자
    public InventoryDTO()
    {
        Items = new List<ItemDTO>();
        EquipItems = new List<ItemDTO>();
    }

    // 생성자: InventorySO에서 데이터를 가져와 DTO로 변환
    public InventoryDTO(InventorySO inventory, Dictionary<EquipmentType, EquipItemSO> equippedItems)
    {
        Items = new List<ItemDTO>();
        EquipItems = new List<ItemDTO>();

        // 인벤토리 아이템 저장
        for (int i = 0; i < inventory.Size; i++)
        {
            InventoryItem item = inventory.GetItemIndex(i);
            Items.Add(new ItemDTO(item));
        }

        // 장착중인 장비 저장
        foreach (var kvp in equippedItems)
        {
            if (kvp.Value != null)
            {
                // ItemIndex와 수량은 1로 저장 (장비는 중복 장착 불가)
                EquipItems.Add(new ItemDTO
                {
                    ItemIndex = kvp.Value.ItemIndex,
                    Quantity = 1,
                    Item = kvp.Value
                });
            }
        }
    }
}

[Serializable]
public class ItemDTO
{
    public int ItemIndex;    // ItemSO ItemIndex   
    public int Quantity;
    public ItemSO Item;

    // 기본 생성자
    public ItemDTO() { }

    // InventoryItem에서 DTO로 변환
    public ItemDTO(InventoryItem item)
    {
        if (item.IsEmpty)
        {
            ItemIndex = 0;
            Quantity = 0;
            Item = null;
        }
        else
        {
            ItemIndex = item.Item.ItemIndex;
            Quantity = item.Quantity;
            Item = item.Item; // ItemSO 참조
        }
    }
}
