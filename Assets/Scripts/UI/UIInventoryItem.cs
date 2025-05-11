using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private bool _empty = true;

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
    /// 데이터 리셋;
    /// 현재는 아이템 이미지만 비활성화하여 하얗게 보이게 만듬;
    /// 추가로 더 커스텀 가능;
    /// </summary>
    public void ResetData()
    {
        this._itemImage.gameObject.SetActive(false);
        _empty = true;
    }

    /// <summary>
    /// 아이템 선택
    /// </summary>
    public void Select()
    {
        _borderImage.enabled = true;
    }

    /// <summary>
    /// 아이템 선택 취소
    /// </summary>
    public void Deselect()
    {
        _borderImage.enabled = false;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        this._itemImage.gameObject.SetActive(true);
        this._itemImage.sprite = sprite;
        this._quantityText.text = quantity + "";
        _empty = false;
    }

    #region Call Event Method
    public void OnPointerClick(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseButtonClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_empty)
            return;

        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);

    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
    #endregion
}
