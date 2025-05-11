using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] UIInventoryItem _itemPrefab;

    [SerializeField] RectTransform _contentPanel;

    [SerializeField] UIInventoryDescription _itemDescription;

    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

    public Sprite Image;
    public int Quantity;
    public string Title;
    public string Description;


    private void Awake()
    {
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

    public void Show()
    {
        gameObject.SetActive(true);
        _itemDescription.ResetDescription();

        listOfUIItems[0].SetData(Image, Quantity);
    }

    public void Hide()
    {
        gameObject?.SetActive(false);
    }

    #region Execute Method
    public void HandleItemSelection(UIInventoryItem item)
    {
        _itemDescription.SetDescription(Image, Title, Description);
        listOfUIItems[0].Select();
    }

    private void HandleBegingDrag(UIInventoryItem item)
    {
        
    }

    private void HandleSwap(UIInventoryItem item)
    {
        
    }

    private void HandleEndDrag(UIInventoryItem item)
    {
        
    }

    private void HandleShowItemActions(UIInventoryItem item)
    {
        
    }
    #endregion
}
