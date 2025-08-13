using System;
using System.Collections.Generic;
using Inventory.Model;

[Serializable]
public class InventoryDTO
{
    // 슬롯별 아이템 정보
    public List<ItemDTO> Items;

    // 기본 생성자
    public InventoryDTO()
    {
        Items = new List<ItemDTO>();
    }

    // 생성자: InventorySO에서 데이터를 가져와 DTO로 변환
    public InventoryDTO(InventorySO inventory)
    {
        Items = new List<ItemDTO>();

        for (int i = 0; i < inventory.Size; i++)
        {
            InventoryItem item = inventory.GetItemIndex(i);
            Items.Add(new ItemDTO(item));
        }
    }
}

[Serializable]
public class ItemDTO
{
    public int ItemID;    // ItemSO ID
    public int Quantity;

    // 기본 생성자
    public ItemDTO() { }

    // InventoryItem에서 DTO로 변환
    public ItemDTO(InventoryItem item)
    {
        if (item.IsEmpty)
        {
            ItemID = 0;
            Quantity = 0;
        }
        else
        {
            ItemID = item.Item.ID;
            Quantity = item.Quantity;
        }
    }
}
