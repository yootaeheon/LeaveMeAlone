using Inventory.Model;
using Inventory.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 현재 장착 중인 장비를 보여주는 UI_Progress
/// </summary>
public class UIEquip : MonoBehaviour
{
    [SerializeField] EquipmentManager _equipManager;

    [SerializeField] InventorySO _inventorySO;

    [SerializeField] Transform _parentTransform;

    [SerializeField] UIInventoryItem _prefab;
 
    [Header("UI_Progress Slot Images")]
    [SerializeField] private Image helmetSlot;
    [SerializeField] private Image armorSlot;
    [SerializeField] private Image backSlot;
    [SerializeField] private Image weaponSlot;

    [Header("비어 있을 때 보여줄 이미지")]
    [SerializeField] private Sprite emptySlotSprite;

    // 장착 중 장비 리스트
    [SerializeField] List<InventoryItem> _equipItems;

    // 인벤토리 슬롯 개수
    [field: SerializeField] public int Size { get; private set; } = 4;

    List<UIInventoryItem> _listOfUIEquip = new List<UIInventoryItem>();    // 현재 페이지 내 아이템 UI_Progress 슬로 리스트

    public void Initialize()
    {
        UpdateAllSlots();
    }

    /// <summary>
    /// 모든 슬롯을 업데이트
    /// </summary>
    public void UpdateAllSlots()
    {
        UpdateSlot(EquipmentType.Helmet, helmetSlot);
        UpdateSlot(EquipmentType.Armor, armorSlot);
        UpdateSlot(EquipmentType.Back, backSlot);
        UpdateSlot(EquipmentType.Weapon, weaponSlot);
    }

    /// <summary>
    /// 각 슬롯을 개별적으로 업데이트
    /// </summary>
    private void UpdateSlot(EquipmentType type, Image image)
    {
        var item = _equipManager.GetEquippedItem(type);
        if (item != null && item.ItemImage != null)
        {
            image.sprite = item.ItemImage;
            /*image.color = Color.white;*/
        }
        else
        {
            image.sprite = emptySlotSprite;
            image.color = new Color(1, 1, 1, 0.4f); // 반투명 처리
        }
    }

   
  
    public void InitInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(_prefab, Vector2.zero, Quaternion.identity);
            uiItem.transform.SetParent(_parentTransform);
            uiItem.transform.localScale = Vector2.one;
            _listOfUIEquip.Add(uiItem);
        }
    }

    public void Init()
    {
        // 장착 중 장비리스트를 새로 생성
        _equipItems = new List<InventoryItem>();

        // 슬롯 개수만큼 비어 있는 아이템으로 채움
        for (int i = 0; i < Size; i++)
        {
            _equipItems.Add(InventoryItem.GetEmptyItem());
        }
    }
}
