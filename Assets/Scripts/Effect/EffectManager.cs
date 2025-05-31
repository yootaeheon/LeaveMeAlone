using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 단일 스택 기반 이펙트 매니저
/// 스택의 가장 위에 있는 이펙트가 요청된 이펙트와 다르면 스택을 비우고 새로 생성
/// 싱글톤으로 작동함 (코루틴으로 리턴시키때문에 Static을 사용할 수 없음(MonoBehaviour를 상속받아야 함))
/// </summary>
public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [SerializeField] EffectSO _effectData;
    public EffectSO EffectData { get { return _effectData; } set { _effectData = value; } }

    // 하나의 공용 스택만 사용, 최대 2개까지 유지
    private Stack<GameObject> effectPool = new();

    private void Awake()
    {
        SetSingleton();
    }

    public void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEffect(GameObject effectPrefab, Vector3 position, Transform followTransform = null, Quaternion rotation = default)
    {
        if (rotation == default)
        {
            rotation = Quaternion.identity;
        }

        if (effectPrefab == null)
        {
            Debug.LogWarning("[EffectManager] Effect Prefab is null.");
            return;
        }

        GameObject effectInstance = null;

        // 스택에 오브젝트가 있고 이름이 일치하면 꺼내기
        if (effectPool.Count > 0 && effectPool.Peek().name == effectPrefab.name)
        {
            effectInstance = effectPool.Pop();
        }
        // 이름이 다르면 풀 비우고 새로 생성
        else
        {
            // 기존 오브젝트들 삭제
            while (effectPool.Count > 0)
            {
                var pooledEffect = effectPool.Pop();
                if (pooledEffect != null)
                {
                    Destroy(pooledEffect);
                }
            }

            effectInstance = Instantiate(effectPrefab);
            effectInstance.name = effectPrefab.name;
        }

        // 위치, 회전, 부모 설정 및 활성화
        effectInstance.transform.SetPositionAndRotation(position, rotation);
        effectInstance.transform.SetParent(followTransform);
        SetActiveRecursively(effectInstance, true);

        float maxLifeTime = CalculateMaxLifetime(effectInstance);
        StartCoroutine(ReturnToPool(effectInstance, maxLifeTime));
    }

    private float CalculateMaxLifetime(GameObject effectObj)
    {
        float maxLifeTime = 0f;

        foreach (ParticleSystem ps in effectObj.GetComponentsInChildren<ParticleSystem>(true))
        {
            var main = ps.main;
            main.playOnAwake = true;
            main.stopAction = ParticleSystemStopAction.Disable;

            float startLifetime = 0f;

            if (main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants)
            {
                startLifetime = Mathf.Max(main.startLifetime.constantMin, main.startLifetime.constantMax);
            }
            else
            {
                startLifetime = main.startLifetime.constant;
            }

            maxLifeTime = Mathf.Max(maxLifeTime, main.duration + startLifetime);
        }

        return maxLifeTime;
    }

    private IEnumerator ReturnToPool(GameObject effectObj, float delay)
    {
        yield return Util.GetDelay(delay);

        if (!effectObj) yield break;

        effectObj.SetActive(false);
        effectObj.transform.SetParent(transform);

        if (effectPool.Count < 2)
        {
            effectPool.Push(effectObj);
        }
        else
        {
            Destroy(effectObj);
        }
    }

    // 풀링된 파티클의 자식 오브젝트 중 일부가 비활성화된 채로 남아 있어서,
    // 다시 활성화되었을 때 전체 이펙트가 제대로 보이지 않는 문제를 수정하는 코드
    private void SetActiveRecursively(GameObject obj, bool isActive)
    {
        obj.SetActive(isActive);
        foreach (Transform child in obj.transform)
        {
            SetActiveRecursively(child.gameObject, isActive);
        }
    }
}
