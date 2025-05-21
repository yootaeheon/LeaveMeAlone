using Inventory.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드래그 앤 드롭 시 아이템 UI 프리팹을 마우스 위치에 띄워주고, 드래그한 아이템 정보를 복사함
/// </summary>
public class MouseFollower : MonoBehaviour
{
    [SerializeField] Canvas _canvas; // 연결된 캔버스
    [SerializeField] UIInventoryItem _item; // 드래그 중 표시할 아이템 UI

    private void Awake()
    {
        // 부모 오브젝트에서 캔버스를 찾아 연결
        _canvas = transform.root.GetComponent<Canvas>();
        _item = GetComponentInChildren<UIInventoryItem>();

        // Overlay 모드에선 worldCamera 필요 없음
    }

    /// <summary>
    /// 아이템 정보를 설정하여 UI에 표시
    /// </summary>
    public void SetData(Sprite sprite, int quantity)
    {
        _item.SetData(sprite, quantity);
    }

    /// <summary>
    /// Overlay 모드에서는 마우스 위치를 곧바로 위치로 설정 가능
    /// </summary>
    private void Update()
    {
        Vector2 screenPos = Input.mousePosition;

        // 마우스 좌표가 유효하지 않은 경우 방어 코드
        if (float.IsInfinity(screenPos.x) || float.IsInfinity(screenPos.y) ||
            float.IsNaN(screenPos.x) || float.IsNaN(screenPos.y))
        {
            return;
        }

        // RectTransform.position은 월드 좌표지만 Overlay에선 스크린 좌표 그대로 사용 가능
        transform.position = screenPos;
    }

    /// <summary>
    /// 마우스 따라다니는 UI 표시 토글
    /// </summary>
    public void Toggle(bool value)
    {
        Debug.Log($"Item Toggled {value}");
        gameObject.SetActive(value);
    }
}
