using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float distance = 10f;
    RaycastHit2D hit; //  2D 전용 RaycastHit

    private void Update()
    {
        // 2D용 Raycast 사용
        hit = Physics2D.Raycast(transform.position, transform.right, distance);

        // 디버그용 선 (Ray가 실제로 맞았는지 확인)
        Debug.DrawRay(transform.position, transform.right * distance, Color.red);

        if (hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            Debug.Log(hit.collider.name);
        }

        if (Input.GetKeyDown(KeyCode.Space) && hit.collider != null)
        {
            // 파티클 재생 위치는 충돌한 위치 기준
            /*EffectManager.Instance.PlayEffect(, hit.point, hit.collider.transform);*/
        }
    }
}
