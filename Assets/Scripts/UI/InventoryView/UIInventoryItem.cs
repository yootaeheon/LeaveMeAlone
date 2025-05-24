using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.View
{
    /// <summary>
    /// 인벤토리 내 아이템 UI_Progress 한 칸을 관리하는 클래스
    // 클릭, 드래그, 드롭 등 마우스 이벤트를 처리
    /// </summary>
    public class UIInventoryItem :
        MonoBehaviour,
        IPointerClickHandler,
        IBeginDragHandler,
        IEndDragHandler,
        IDropHandler,
        IDragHandler
    {
        [SerializeField] Image _itemImage; // 아이템 이미지

        [SerializeField] TMP_Text _quantityText; // 보유 수량

        [SerializeField] Image _borderImage; //아이템 선택 표시 테두리

        private bool _empty = true; // 이 칸이 비어있는지 여부

        // 외부에서 구독할 수 있는 이벤트 정의 (아이템 클릭, 드래그 시작/끝, 드롭, 우클릭 등)
        public event Action<UIInventoryItem>
            OnItemClicked,
            OnItemDroppedOn,
            OnItemBeginDrag,
            OnItemEndDrag,
            OnRightMouseButtonClick;

        public void Awake()
        {
            ResetData();           
            Deselect();            
        }

        /// <summary>
        /// 아이템 데이터를 초기화한다.
        /// 현재는 아이템 이미지 숨기고 비어있는 상태로 표시.
        /// </summary>
        public void ResetData()
        {
            _itemImage.gameObject.SetActive(false);
            _empty = true;
        }

        /// <summary>
        /// 아이템 선택 시, 테두리 활성화하여 선택 상태 표시
        /// </summary>
        public void Select()
        {
            _borderImage.enabled = true;
        }

        /// <summary>
        /// 아이템 선택 취소 시, 테투리 비활성화
        /// </summary>
        public void Deselect()
        {
            _borderImage.enabled = false;
        }

        /// <summary>
        /// 아이템 이미지 및 수량 UI에 데이터 세팅
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="quantity"></param>
        public void SetData(Sprite sprite, int quantity)
        {
            _itemImage.gameObject.SetActive(true);
            _itemImage.sprite = sprite;
            _quantityText.text = quantity + "";
            _empty = false;
        }

        #region 마우스 이벤트 핸들러
        /// <summary>
        /// 클릭/터치 처리
        /// </summary>
        /// <param name="pointerData"></param>
        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                // 우클릭 시 우클릭 이벤트 발생
                OnRightMouseButtonClick?.Invoke(this);
            }
            else
            {
                // 좌클릭 등 일반 클릭 시 이벤트 발생
                OnItemClicked?.Invoke(this);
            }
        }

        /// <summary>
        /// 드래그 시작 시 호출
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            // 빈 슬롯이면 return
            if (_empty)
                return;

            OnItemBeginDrag?.Invoke(this);
        }

        /// <summary>
        /// 드래그 종료 시 호출
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        /// <summary>
        /// 다른 슬롯 위에 드롭했을 때 호출
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        /// <summary>
        /// 드래그 중 마우스 이동 시  호출
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {

        }
        #endregion
    }
}