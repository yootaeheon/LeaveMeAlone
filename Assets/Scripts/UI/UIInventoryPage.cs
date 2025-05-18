using Inventory.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.View
{
    // UI → InventoryController → InventorySO → UI 의 흐름으로 움직이며,
    // 이벤트를 통해 UI와 데이터 모델 간 데이터 요청, 변경, 갱신이 이루어집니다.

    /// <summary>
    /// 인벤토리 UI 한 페이지 전체 관리 클래스
    /// 1. 아이템 UI 슬롯 생성
    /// 2. 선택
    /// 3. 드래그 앤 드롭
    /// 4. 설명 표시
    /// 5. 액션 요청
    /// </summary>
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] UIInventoryItem _itemPrefab;

        [SerializeField] RectTransform _contentPanel;

        [SerializeField] UIInventoryDescription _itemDescription;

        [SerializeField] MouseFollower _mouseFollower;

        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();    // 현재 페이지 내 아이템 UI 슬로 리스트

        private int curDraggedIndex = -1; // 현재 드래그 중인 아이템 인덱스

        /// <summary>
        /// 외부에서 구독 하는 이벤트
        /// </summary>
        public event Action<int>
            OnDescriptionRequested,    // 어아탬 설명 요청
            OnItemActionRequested,     // 아이템 액션 요청 시 (우클릭 메뉴) // 우클릭 기능 삭제 예정
            OnStartDragging;           // 드래그 시작 시

        public event Action<int, int> OnSwapItems; // 아이템 위치 변경 요청 시(드래그-드롭)


        private void Awake()
        {
            Hide();
            _mouseFollower.Toggle(false);
            _itemDescription.ResetDescription();
        }

        /// <summary>
        /// 인벤토리 UI 슬롯 초기화 및 생성
        /// </summary>
        /// <param name="inventorySize"></param>
        public void InityInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(_itemPrefab, Vector2.zero, Quaternion.identity);
                uiItem.transform.SetParent(_contentPanel);
                uiItem.transform.localScale = Vector2.one;
                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += SelectItem;
                uiItem.OnItemBeginDrag += BegingDrag;
                uiItem.OnItemDroppedOn += Swap;
                uiItem.OnItemEndDrag += EndDrag;
                uiItem.OnRightMouseButtonClick += ShowItemActions;
            }
        }

        /// <summary>
        /// 인벤토리 UI 활성화 및 선택 초기화
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        /// <summary>
        /// 인벤토리 UI 비활성화 및 드래그 상태 초기화
        /// </summary>
        public void Hide()
        {
            gameObject?.SetActive(false);
            ResetDraggedItem();
        }

        /// <summary>
        /// 아이템 설명 초기화 및 모든 아이템 선택 해제
        /// </summary>
        public void ResetSelection()
        {
            _itemDescription.ResetDescription();
            DeselectAllItems();
        }

        /// <summary>
        /// 모든 아이템 UI 슬롯의 선택 해제 처리
        /// </summary>
        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
        }

        /// <summary>
        /// 특정 인덱스의 아이템 UI 데이터를 갱신한다.
        /// </summary>
        /// <param name="itemIndex">아이템 인덱스</param>
        /// <param name="itemImage">아이템 이미지</param>
        /// <param name="itemQuantity">아이템 수량</param>
        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        /// <summary>
        /// 특정 인덱스 아이템 설명 UI 갱신
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="itemImage"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            _itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        #region Execute Method
        /// <summary>
        /// 아이템 슬롯 선택 시 호출
        /// 선택한 아이템 인덱스를 찾아 아이템 설명 요청
        /// </summary>
        /// <param name="item"></param>
        public void SelectItem(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1)
                return;

            OnDescriptionRequested?.Invoke(index);
        }

        /// <summary>
        /// 아이템 드래그 시작 시 호출
        /// 현재 드래그 중인 아이템 인덱스 저장 후,
        /// 설명 요청하고, 
        /// 드래그 중 이벤트 발생
        /// </summary>
        /// <param name="item"></param>
        private void BegingDrag(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1)
                return;

            curDraggedIndex = index;
            SelectItem(item);
            OnStartDragging?.Invoke(index);
        }

        

        /// <summary>
        /// 아이템 드래그 앤 드롭을 통한 자리바꿈 이벤트 발생하는 메서드
        /// </summary>
        /// <param name="item"></param>
        private void Swap(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(curDraggedIndex, index);
        }


        /// <summary>
        /// MouseFollower 활성화 후,
        /// 마우스 위치에 똑같이 복사
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="quantity"></param>
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            _mouseFollower.Toggle(true);
            _mouseFollower.SetData(sprite, quantity);
        }


        /// <summary>
        /// MouseFollower 비활성 화 후,
        /// 선택 중인 아이템 인덱스 -1로 리셋
        /// </summary>
        private void ResetDraggedItem()
        {
            _mouseFollower.Toggle(false);
            curDraggedIndex = -1;
        }

        private void EndDrag(UIInventoryItem item)
        {
            ResetDraggedItem();
        }

        /// <summary>
        /// 아이템 우클릭 액션 요청 시 이벤트 발생
        /// 우클릭 기능 삭제 예정
        /// </summary>
        /// <param name="item"></param>
        private void ShowItemActions(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }
        #endregion
    }
}