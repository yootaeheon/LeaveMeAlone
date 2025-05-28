using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShopCanvas : MonoBehaviour
{
    /// <summary>
    /// 상점 Canvas UI_Progress 활성화
    /// </summary>
    public void Button_Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 상점 Canvas UI_Progress 비활성화
    /// </summary>
    public void Button_Hide()
    {
        gameObject.SetActive(false);
    }
}
