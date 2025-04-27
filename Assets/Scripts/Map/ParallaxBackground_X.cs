using UnityEngine;

public class ParallaxBackground_X : MonoBehaviour
{
    [SerializeField] Transform _target;            // 현재 배경과 이어지는 배경

    [SerializeField] float _scrollAmount;          // 이어지는 두 배경 사이 거리

    [SerializeField] float _moveSpeed;             // 이동 속도

    [SerializeField] Vector3 _moveDirection;       // 이동 방향

    private void Update()
    {
        // 배경이 _moveDirection 방향으로 _moveSpeed 속도로 이동
        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;

        // 배경이 설정된 범위를 벗어나면 위치 재설정
        if (transform.position.x <= -_scrollAmount)
        {
            // 이동 방향의 반대 방향 * _scrollAmount만큼 간격으로 위치 재설정
            transform.position = _target.position - _moveDirection * _scrollAmount;
        }
    }

}
