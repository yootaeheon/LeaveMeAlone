using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxBackground : MonoBehaviour
{


    [SerializeField] float[] layerMoveSpeed;         // z 값이 다른 배경 레이어 별 이동 속도

    [SerializeField] CharacterController _characterController;

    private int backgroundCount;                     // Layer 수

    private Material[] materials;                    // 배경 스크롤을 위한 Material 배열 변수

    private void Awake()
    {
        // 배경의 개수를 구하고, 배경 정보를 저장할 GameObject 배열 선언
        backgroundCount = transform.childCount;
        GameObject[] backgrounds = new GameObject[backgroundCount];

        // 각 배경의 material과 이동 속도를 저장할 배열 선언
        materials = new Material[backgroundCount];
        layerMoveSpeed = new float[backgroundCount];


        for (int i = 0; i < backgroundCount; ++i)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        SetLayerMoveSpeed();
    }

    private void OnEnable()
    {
        _characterController.OnEncounterMonster += ResetLayerMoveSpeed;
        _characterController.OnKillMonster += SetLayerMoveSpeed;
    }

    private void OnDisable()
    {
        _characterController.OnEncounterMonster -= ResetLayerMoveSpeed;
        _characterController.OnKillMonster -= SetLayerMoveSpeed;

    }

    /// <summary>
    /// 각 Layer 스크롤
    /// </summary>
    private void Update()
    {
        for (int i = 1; i < materials.Length; ++i)
        {
            materials[i].SetTextureOffset("_MainTex", Vector2.right * layerMoveSpeed[i] * Time.time);
        }
    }

    /// <summary>
    /// 각 Layer의 moveSpeed 설정
    /// </summary>
    public void SetLayerMoveSpeed()
    {
        float stackSpeed = 0.01f;
        for (int i = 1; i < backgroundCount; i++)
        {
            layerMoveSpeed[i] = stackSpeed;
            stackSpeed += 0.01f;
        }
    }

    public void ResetLayerMoveSpeed()
    {
        for (int i = 1; i < backgroundCount; i++)
        {
            layerMoveSpeed[i] = 0;
        }
    }

}