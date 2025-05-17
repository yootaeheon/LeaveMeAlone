using Inventory.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드래그 앤 드롭 시 아이템UI프리팹을 투명하게 마우스 위치로 띄워주고 그 UI에 드래그한 아이템을 똑같이 세팅해줌
/// </summary>
public class MouseFollower : MonoBehaviour
{
    [SerializeField] Canvas _canvas;

    [SerializeField] UIInventoryItem _item;

    private void Awake()
    {
        _canvas = transform.root.GetComponent<Canvas>();
        _item = GetComponentInChildren<UIInventoryItem>();

        // worldCamera가 null일 경우 Camera.main을 할당
        if (_canvas.worldCamera == null) _canvas.worldCamera = Camera.main;
    }

    /// <summary>
    /// 드래그한 아이템을 마우스 위치에 똑같이 초기화;
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="quantity"></param>
    public void SetData(Sprite sprite, int quantity)
    {
        _item.SetData(sprite, quantity);
    }

    /// <summary>
    /// 마우스 포인터와 2D 상의 캔버스의 RectTransform 위치 변환을 수행
    /// 모바일 환경과 에디터상에서 마우스의 위치가 상시 존재하지 않거나 할당되지 않는 위치로 갔을 때 에러가 발생하므로 조건 체크해줌.
    /// </summary>
    private void Update()
    {
        if (_canvas.worldCamera == null) return;

        Vector2 screenPos = Input.mousePosition;

        // 마우스 위치가 유효하지 않은 경우 early return
        if (float.IsInfinity(screenPos.x) || float.IsInfinity(screenPos.y) ||
            float.IsNaN(screenPos.x) || float.IsNaN(screenPos.y))
        {
            return;
        }

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_canvas.transform,
                screenPos,
                _canvas.worldCamera,
                out pos))
        {
            transform.position = _canvas.transform.TransformPoint(pos);
        }
    }

    /// <summary>
    /// Toggle이 True일 때만 마우스 위치에 투명한 프리팹이 보임
    /// </summary>
    /// <param name="value"></param>
    public void Toggle(bool value)
    {
        Debug.Log($"Item Toggled {value}");
        gameObject.SetActive(value);
    }
}
