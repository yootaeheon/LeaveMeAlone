using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        // 인벤토리에 들어가는 아이템들의 리스트
        [SerializeField] List<InventoryItem> _inventoryItems;

        // 인벤토리 슬롯 개수
        [field: SerializeField] public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

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

        public void AddItem(InventoryItem item)
        {
            AddItem(item.Item, item.Quantity);
        }

        /// <summary>
        /// 아이템을 인벤토리에 추가하는 함수
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public int AddItem(ItemSO item, int quantity)
        {
            if (item.IsStackable == false)
            {
                // 인벤토리의 각 슬롯을 확인
                for (int i = 0; i < _inventoryItems.Count; i++)
                {
                    while (quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstSlot(item, 1);
                    }
                    return quantity;

                }
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFirstSlot(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem()
            {
                Item = item,
                Quantity = quantity
            };

            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                {
                    _inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        /// <summary>
        /// 인벤토리가 가득찼는지 확인
        /// </summary>
        /// <returns></returns>
        private bool IsInventoryFull()
            => _inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i].IsEmpty)
                    continue;

                if (_inventoryItems[i].Item.ID == item.ID)
                {
                    int amountPossibleToTake = _inventoryItems[i].Item.MaxStackSize - _inventoryItems[i].Quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i].Item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        _inventoryItems[i] = _inventoryItems[i].ChangeQuantity(_inventoryItems[i].Quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstSlot(item, newQuantity);
            }
            return quantity;
        }

       

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurInventoryDic());
        }

        /// <summary>
        /// 현재 인벤토리 상태를 Dictionary 형태로 반환 (빈 슬롯은 제외)
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, InventoryItem> GetCurInventoryDic()
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

        public InventoryItem GetItemIndex(int itemIndex)
        {
            return _inventoryItems[itemIndex];
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if (_inventoryItems.Count > itemIndex)
            {
                if (_inventoryItems[itemIndex].IsEmpty)
                    return;

                int reminder = _inventoryItems[itemIndex].Quantity - amount;
                if (reminder <= 0)
                {
                    _inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                }
                else
                {
                    _inventoryItems[itemIndex] = _inventoryItems[itemIndex].ChangeQuantity(reminder);
                } 

                InformAboutChange();
            }
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = _inventoryItems[itemIndex_1];
            _inventoryItems[itemIndex_1] = _inventoryItems[itemIndex_2];
            _inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
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
}