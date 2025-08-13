using System;
using System.Collections.Generic;
using Inventory.Model;

[Serializable]
public class InventoryDTO
{
    // ���Ժ� ������ ����
    public List<ItemDTO> Items;

    // �⺻ ������
    public InventoryDTO()
    {
        Items = new List<ItemDTO>();
    }

    // ������: InventorySO���� �����͸� ������ DTO�� ��ȯ
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

    // �⺻ ������
    public ItemDTO() { }

    // InventoryItem���� DTO�� ��ȯ
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
