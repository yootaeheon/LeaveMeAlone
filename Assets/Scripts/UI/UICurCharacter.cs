using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurCharacter : MonoBehaviour
{
    public void OnOffCanvas()
    {
        if (gameObject.activeSelf == false)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    /// <summary>
    /// 스탯창 UI 활성화
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 스탯창 UI 비활성화
    /// </summary>
    public void Hide()
    {
        gameObject?.SetActive(false);
    }
}
