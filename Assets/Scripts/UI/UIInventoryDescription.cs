using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField] Image _itemImage;

    [SerializeField] TMP_Text _title;

    [SerializeField] TMP_Text _description;

    private void Awake()
    {
        ResetDescription();
    }

    /// <summary>
    /// 아이템 프리팹 백지로 초기화
    /// </summary>
    public void ResetDescription()
    {
        this._itemImage.gameObject.SetActive(false);
        this._title.text = "";
        this._description.text = "";
    }

    /// <summary>
    /// 아이템 스프라이트, 이름, 설명 초기화
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="itemName"></param>
    /// <param name="itemDescription"></param>
    public void SetDescription(Sprite sprite, string itemName, string itemDescription)
    {
        this._itemImage.gameObject.SetActive(true);
        this._itemImage.sprite = sprite;
        this._title.text = itemName;
        this._description.text = itemDescription;
    }
}
