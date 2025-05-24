using Inventory.Model;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using Inventory.View;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// InventoryPage(UI_Progress)와 InventorySO(데이터 모델)를 중개하는 컨트롤러 역할
    /// </summary>
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] UIInventoryPage _inventoryUI;       // UI_Progress
                                                          
        [SerializeField] InventorySO _inventoryData;         // Data

        public List<InventoryItem> _initItems = new List<InventoryItem>();

        private void Awake()
        {
            PrepareUI();
            /* _inventoryData.Init();*/
            PrepareInventoryData();
        }

        private void OnDisable()
        {
            _inventoryData.OnInventoryUpdated -= UpdateInventoryUI;

            _inventoryUI.OnDescriptionRequested -= CallRequestDescription;  // 설명 요청 이벤트 구독
            _inventoryUI.OnSwapItems -= CallSwapItems;                      // 아이템 교환 이벤트 구독
            _inventoryUI.OnStartDragging -= CallDragging;                   // 드래그 시작 시 이벤트 구독
            _inventoryUI.OnItemActionRequested -= CallItemActionRequest;    // 아이템 액션 요청 이벤트 구독
        }

        private void PrepareUI()
        {
            _inventoryUI.InitInventoryUI(_inventoryData.Size);               // UI_Progress 슬롯 개수 초기화

            _inventoryUI.OnDescriptionRequested += CallRequestDescription;  // 설명 요청 이벤트 구독
            _inventoryUI.OnSwapItems += CallSwapItems;                      // 아이템 교환 이벤트 구독
            _inventoryUI.OnStartDragging += CallDragging;                   // 드래그 시작 시 이벤트 구독
            _inventoryUI.OnItemActionRequested += CallItemActionRequest;    // 아이템 액션 요청 이벤트 구독
        }

        public void PrepareInventoryData()
        {
            _inventoryData.Init();
            _inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in _initItems)
            {
                if (item.IsEmpty)
                    continue;
                _inventoryData.AddItem(item);
            }
        }

        public void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            _inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                _inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);
            }
        }

        /// <summary>
        /// 인벤토리 열기/닫기 메서드
        /// 인벤토리 버튼에 연결
        /// </summary>
        public void OnOffInventory()
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

       

        #region InventoryUI에 문의하여 모델 데이터 접근
        /// <summary>
        /// 아이템 설명 요청 처리
        /// 해당 인덱스 아이템 데이터 가져와서 UI에 설명 업데이트
        /// 아이템이 비었으면 선택 초기화
        /// </summary>
        /// <param name="itemIndex"></param>
        private void CallRequestDescription(int itemIndex)
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

        /// <summary>
        /// 아이템 위치 교환 처리
        /// InventorySO(데이터 모델)에 아이템 스왑 메서드 호출
        /// </summary>
        /// <param name="itemIndex_1"></param>
        /// <param name="itemIndex_2"></param>
        private void CallSwapItems(int itemIndex_1, int itemIndex_2)
        {
            _inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        /// <summary>
        /// 드래그 시작 시 처리
        /// 드래그하는 아이템의 이미지와 수량 정보를 MouserFollwer에 똑같이 생성
        /// </summary>
        /// <param name="itemIndex"></param>
        private void CallDragging(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemIndex(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            _inventoryUI.CreateDraggedItem(inventoryItem.Item.ItemImage, inventoryItem.Quantity);
        }

        /// <summary>
        /// 아이템 사용 등 액션 요청 처리
        /// 아이템이 비었으면 선택해제
        /// 
        /// 그렇지 않으면 아이템 설명 업데이트 및 아이템 액션 호출
        /// 필요시 아이템 소모 처리
        /// </summary>
        /// <param name="itemIndex"></param>
        private void CallItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = _inventoryData.GetItemIndex(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                _inventoryUI.ResetSelection();
            }

            ItemSO item = inventoryItem.Item;
            _inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
        }
        #endregion
    }
}