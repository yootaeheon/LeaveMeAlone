using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장비 아이템 장착/해제/조회 관리하는 클래스
/// 각 EquipmentType에 따라 하나의 아이템만 장착 가능
/// </summary>
public class EquipmentManager : MonoBehaviour
{
    /// <summary>
    /// 현재 장착중인 아이템들 저장하는 딕셔너리
    /// 키 : 장비 타입 (Ex> Aromor, Weapon 등)
    /// 값 : 해당 타입에 장착한 EquipItemSO
    /// </summary>
    private Dictionary<EquipmentType, EquipItemSO> _equippedItems = new();

    /// <summary>
    /// 장비 장착 메서드
    /// </summary>
    /// <param name="item"></param>
    public void EquipItem(EquipItemSO item)
    {
        _equippedItems[item.EquipmentType] = item;
        // 스탯 반영 로직 등...
    }

    /// <summary>
    /// 장비 장착 해제 메서드
    /// 장착중인 장비 딕셔너리에서 제거
    /// </summary>
    /// <param name="type"></param>
    public void UnequipItem(EquipmentType type)
    {
        if (_equippedItems.ContainsKey(type))
            _equippedItems.Remove(type);
    }

    /// <summary>
    /// 특정 EquipmentType에 장착된 아이템을 반환
    /// 없으면 null 반환.
    /// </summary>
    /// <param name="type">조회할 장비의 타입</param>
    /// <returns>장착된 장비 아이템 또는 null</returns>
    public EquipItemSO GetEquippedItem(EquipmentType type)
    {
        _equippedItems.TryGetValue(type, out EquipItemSO item);
        return item;
    }
}
