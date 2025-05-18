using Inventory.Model;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using Inventory.View;

namespace Inventory
{
    /// <summary>
    /// InventoryPage(UI)와 InventorySO(데이터 모델)를 중개하는 컨트롤러 역할
    /// </summary>
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] UIInventoryPage _inventoryUI;       // UI
                                                          
        [SerializeField] InventorySO _inventoryData;         // Data

        private void Start()
        {
            PrepareUI();
            /* _inventoryData.Init();*/
        }
        private void PrepareUI()
        {
            _inventoryUI.InityInventoryUI(_inventoryData.Size);               // UI 슬롯 개수 초기화

            _inventoryUI.OnDescriptionRequested += RequestDescription;  // 설명 요청 이벤트 구독
            _inventoryUI.OnSwapItems += HandleSwapItems;                      // 아이템 교환 이벤트 구독
            _inventoryUI.OnStartDragging += HandleDragging;                   // 드래그 시작 시 이벤트 구독
            _inventoryUI.OnItemActionRequested += HandleItemActionRequest;    // 아이템 액션 요청 이벤트 구독
        }

        /// <summary>
        /// 인벤토리 열기/닫기 메서드
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

       

        #region InventoryUI에 문의하여 모델 데이터 접근
        /// <summary>
        /// 아이템 설명 요청 처리
        /// 해당 인덱스 아이템 데이터 가져와서 UI에 설명 업데이트
        /// 아이템이 비었으면 선택 초기화
        /// </summary>
        /// <param name="itemIndex"></param>
        private void RequestDescription(int itemIndex)
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
        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            _inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        /// <summary>
        /// 드래그 시작 시 처리
        /// 드래그하는 아이템의 이미지와 수량 정보를 MouserFollwer에 똑같이 생성
        /// </summary>
        /// <param name="itemIndex"></param>
        private void HandleDragging(int itemIndex)
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
        private void HandleItemActionRequest(int itemIndex)
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