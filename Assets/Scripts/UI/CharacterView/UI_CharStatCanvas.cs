using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharStatCanvas : MonoBehaviour
{
    /// <summary>
    /// 스탯창 Canvas UI_Progress 활성화
    /// </summary>
    public void Button_Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 스탯창 Canvas UI_Progress 비활성화
    /// </summary>
    public void Button_Hide()
    {
        gameObject.SetActive(false);
    }
}
