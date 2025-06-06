using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Item.ElementTypeEnum;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] CharacterModel Model;

    [SerializeField] InventorySO InventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.gameObject.GetComponent<Item>();
            if (item != null)
            {
                int reminder = InventoryData.AddItem(item.InventoryItem, item.Quantity);

                if (reminder == 0)
                {
                    item.DestroyItem();
                }
                else
                {
                    item.Quantity = reminder;
                }
            }

          
            if (item.InventoryItem is ConsumeItemSO consumeItem)
            {
                AddElement(consumeItem.ElementType);
            }
        }
    }

    public void AddElement(ElementType type)
    {
        Model.ElementType = type;
        Console.WriteLine($"속성 추가됨: {type}, 현재 속성: {Model.ElementType}");
    }

    // 특정 속성을 가지고 있는지 확인
    public bool HasElement(ElementType type)
    {
        return (Model.ElementType & type) != 0;
    }

    // 속성 제거 (예: 저주받은 아이템 등)
    public void RemoveElement(ElementType type)
    {
        Model.ElementType &= ~type;
        Console.WriteLine($"속성 제거됨: {type}, 현재 속성: {Model.ElementType}");
    }
}
