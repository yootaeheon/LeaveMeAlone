using Inventory.Model;
using Inventory.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 현재 장착 중인 장비를 보여주는 UI_Progress
/// </summary>
public class UIEquip : MonoBehaviour
{
    [SerializeField] EquipmentManager _equipManager;

    [Header("curItem Sprite")]
    [SerializeField] private Image _helmetImage;
    [SerializeField] private Image _armorSpriteRenderer;
    [SerializeField] private Image _backSpriteRenderer;
    [SerializeField] private Image _weaponSpriteRenderer;

    [Header("비어 있을 때 보여줄 이미지")]
    [SerializeField] private Sprite emptySlotSprite;

    // 장착 중 장비 리스트
    [SerializeField] List<InventoryItem> _equipItems = new List<InventoryItem>();

    List<UIInventoryItem> _listOfUIEquip = new List<UIInventoryItem>();    // 현재 페이지 내 아이템 UI_Progress 슬로 리스트

    private void Awake()
    { 
Init();
    }

    public void OnEnable()
{
    UpdateAllSlots();
}

/// <summary>
/// 모든 슬롯을 업데이트
/// </summary>
public void UpdateAllSlots()
{
    UpdateSlot(EquipmentType.Helmet, _helmetImage);
    UpdateSlot(EquipmentType.Armor, _armorSpriteRenderer);
    UpdateSlot(EquipmentType.Back, _backSpriteRenderer);
    UpdateSlot(EquipmentType.Weapon, _weaponSpriteRenderer);
}

/// <summary>
/// 각 슬롯을 개별적으로 업데이트
/// </summary>
public void UpdateSlot(EquipmentType type, Image image)
{
    var item = _equipManager.GetEquippedItem(type);
    if (item != null && item.ItemImage != null)
    {
        image.sprite = item.ItemImage;
            image.color = Color.white;
        }
    else
    {
        image.sprite = emptySlotSprite;
        image.color = new Color(1, 1, 1, 0.4f); // 반투명 처리
    }
}

public void Init()
{
    // 장착 중 장비리스트를 새로 생성
    /*_equipItems = new List<InventoryItem>();*/

    // 슬롯 개수만큼 비어 있는 아이템으로 채움
    for (int i = 0; i < 4; i++)
    {
        _equipItems.Add(InventoryItem.GetEmptyItem());
    }
}
}
