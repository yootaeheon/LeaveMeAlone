using Inventory.Model;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using Inventory.View;

namespace Inventory
{
    /// <summary>
    /// InventoryPage와 InventorySO를 중개해줌;
    /// </summary>
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] UIInventoryPage _inventoryUI;

        [SerializeField] InventorySO _inventoryData;

        private void Start()
        {
            PrepareUI();
            /* _inventoryData.Init();*/
        }

        /// <summary>
        /// 인벤토리 열기 메서드
        /// 인벤토리 버튼에 연결
        /// </summary>
        public void OnInventory()
        {
            if (_inventoryUI.isActiveAndEnabled == false)
            {
                _inventoryUI.Show();
                foreach (var item in _inventoryData.GetCurInventoryDic())
                {
                    _inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);
                }
            }
            else
            {
                _inventoryUI.Hide();
            }
        }

        private void PrepareUI()
        {
            _inventoryUI.InityInventoryUI(_inventoryData.Size);
            _inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            _inventoryUI.OnSwapItems += HandleSwapItems;
            _inventoryUI.OnStartDragging += HandleDragging;
            _inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        #region InventoryUI에 문의하여 모델 데이터 접근
        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemIndex(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                _inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.Item;
            _inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            _inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemIndex(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            _inventoryUI.CreateDraggedItem(inventoryItem.Item.ItemImage, inventoryItem.Quantity);
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemIndex(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                _inventoryUI.ResetSelection();
            }

            ItemSO item = inventoryItem.Item;
            _inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);

             IItemAction itemAction = inventoryItem.Item as IItemAction;
            if (itemAction != null)
            {
                itemAction.Use();
            }
            IDestoryableItem destroyableItem = inventoryItem.Item as IDestoryableItem;
            if (destroyableItem != null)
            {
                _inventoryData.RemoveItem(itemIndex, 1);
            }
        }
        #endregion

    }
}