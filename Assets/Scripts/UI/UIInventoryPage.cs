using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] UIInventoryItem _itemPrefab;

    [SerializeField] RectTransform _contentPanel;

    [SerializeField] UIInventoryDescription _itemDescription;

    [SerializeField] MouseFollower _mouseFollower;

    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    private int curDraggedIndex = -1;

    public event Action<int>
        OnDescriptionRequested,
        OnItemActionRequested,
        OnStartDragging;

    public event Action<int, int> OnSwapItems;

    private void Awake()
    {
        Hide();
        _mouseFollower.Toggle(false);
        _itemDescription.ResetDescription();
    }

    public void InityInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(_itemPrefab, Vector2.zero, Quaternion.identity);
            uiItem.transform.SetParent(_contentPanel);
            uiItem.transform.localScale = Vector2.one;
            listOfUIItems.Add(uiItem);

            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBegingDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseButtonClick += HandleShowItemActions;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }

    public void Hide()
    {
        gameObject?.SetActive(false);
        ResetDraggedItem();
    }

    public void ResetSelection()
    {
        _itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in listOfUIItems)
        {
            item.Deselect();
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfUIItems.Count > itemIndex)
        {
            listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        _itemDescription.SetDescription(itemImage, name, description);
        DeselectAllItems();
        listOfUIItems[itemIndex].Select();
    }

    #region Execute Method
    public void HandleItemSelection(UIInventoryItem item)
    {
        int index = listOfUIItems.IndexOf(item);
        if (index == -1)
            return;

        OnDescriptionRequested?.Invoke(index);
    }

    private void HandleBegingDrag(UIInventoryItem item)
    {
        int index = listOfUIItems.IndexOf(item);
        if (index == -1)
            return;

        curDraggedIndex = index;
        HandleItemSelection(item);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        _mouseFollower.Toggle(true);
        _mouseFollower.SetData(sprite, quantity);
    }

    private void HandleSwap(UIInventoryItem item)
    {
        int index = listOfUIItems.IndexOf(item);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(curDraggedIndex, index);
    }

    private void ResetDraggedItem()
    {
        _mouseFollower.Toggle(false);
        curDraggedIndex = -1;
    }

    private void HandleEndDrag(UIInventoryItem item)
    {
        ResetDraggedItem();
    }

    private void HandleShowItemActions(UIInventoryItem item)
    {
        
    }

    
    #endregion
}
