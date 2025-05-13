using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    // 인벤토리에 들어가는 아이템들의 리스트
    [SerializeField] List<InventoryItem> _inventoryItems;

    // 인벤토리 슬롯 개수
    [field: SerializeField] public int Size { get; private set; } = 10;

    /// <summary>
    /// 인벤토리를 초기화하는 함수
    /// </summary>
    public void Init()
    {
        // 아이템 리스트를 새로 생성
        _inventoryItems = new List<InventoryItem>();

        // 슬롯 개수만큼 비어 있는 아이템으로 채움
        for (int i = 0; i < Size; i++)
        {
            _inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    /// <summary>
    /// 아이템을 인벤토리에 추가하는 함수
    /// </summary>
    /// <param name="item"></param>
    /// <param name="quantity"></param>
    public void AddItem(ItemSO item, int quantity)
    {
        // 인벤토리의 각 슬롯을 확인
        for (int i = 0; i < _inventoryItems.Count; i++)
        {
            // 비어 있는 슬롯을 찾으면
            if (_inventoryItems[i].IsEmpty)
            {
                // 해당 슬롯에 아이템을 추가
                _inventoryItems[i] = new InventoryItem()
                {
                    Item = item,
                    Quantity = quantity
                };
                // 아이템을 한 번 추가한 뒤 반복문 종료 (중복 추가 방지)
                break;
            }
        }
    }

    /// <summary>
    /// 현재 인벤토리 상태를 Dictionary 형태로 반환 (빈 슬롯은 제외)
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, InventoryItem> GetCurInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

        for (int i = 0; i < _inventoryItems.Count; i++)
        {
            // 빈 슬롯은 건너뜀
            if (_inventoryItems[i].IsEmpty)
                continue;

            // 슬롯 번호(index)와 해당 아이템을 딕셔너리에 추가
            returnValue[i] = _inventoryItems[i];
        }

        // 현재 아이템이 있는 슬롯만 포함된 딕셔너리 반환
        return returnValue;
    }
}

/// <summary>
/// 인벤토리에 들어가는 개별 아이템 구조체
/// </summary>
[Serializable]
public struct InventoryItem  // 구조체 사용: 값 타입이므로 다른 스크립트에서 실수로 수정되는 것을 방지
{
    public int Quantity;      // 아이템 수량
    public ItemSO Item;       // 아이템 데이터 (ScriptableObject)

    // 아이템이 비어 있는지 여부 확인 (Item이 null이면 비어 있음)
    public bool IsEmpty => Item == null;

    /// <summary>
    /// 수량을 변경한 새로운 InventoryItem을 반환 (값 타입이므로 직접 수정 불가)
    /// </summary>
    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem()
        {
            Item = this.Item,
            Quantity = newQuantity,
        };
    }

    /// <summary>
    /// 빈 아이템을 생성하는 정적 함수
    /// </summary>
    /// <returns></returns>
    public static InventoryItem GetEmptyItem()
        => new InventoryItem()
        {
            Item = null,
            Quantity = 0,
        };
}
