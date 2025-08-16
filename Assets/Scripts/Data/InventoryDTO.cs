using Inventory.Model;
using System;
using System.Collections.Generic;

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
    public int ItemIndex;    // ItemSO ItemIndex   
    public int Quantity;
    public ItemSO Item;

    // �⺻ ������
    public ItemDTO() { }

    // InventoryItem���� DTO�� ��ȯ
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
            Item = item.Item; // ItemSO ����
        }
    }
}
