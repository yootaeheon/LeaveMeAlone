using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ResourcesManager
{
    /// <summary>
    /// ��巹���� ������ �񵿱�� �ε� (T Ÿ�� ��ȯ)
    /// </summary>
    public static async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken ct = default) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        await handle.ToUniTask(cancellationToken: ct);
        if (handle.Status == AsyncOperationStatus.Succeeded)
            return handle.Result;
        Debug.LogError($"[ResourcesManager] ���� �ε� ����: {key}");
        return null;
    }

    /// <summary>
    /// ��巹���� �������� �񵿱�� �ε� �� �ν��Ͻ�ȭ (GameObject ��ȯ)
    /// </summary>
    public static async UniTask<GameObject> InstantiateAsync(string key, Transform parent = null, bool worldPositionStays = true, CancellationToken ct = default)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, parent, worldPositionStays);
        await handle.ToUniTask(cancellationToken: ct);
        if (handle.Status == AsyncOperationStatus.Succeeded)
            return handle.Result;
        Debug.LogError($"[ResourcesManager] �ν��Ͻ�ȭ ����: {key}");
        return null;
    }

    /// <summary>
    /// ��巹���� ���� ����
    /// </summary>
    public static void Release<T>(T obj) where T : UnityEngine.Object
    {
        Addressables.Release(obj);
    }

    /// <summary>
    /// ��巹���� �ν��Ͻ� ����
    /// </summary>
    public static void ReleaseInstance(GameObject go)
    {
        Addressables.ReleaseInstance(go);
    }
}

/*// Usage example
var prefab = await ResourcesManager.LoadAssetAsync<GameObject>("MyPrefab");
var go = await ResourcesManager.InstantiateAsync("MyPrefab", parentTransform);
ResourcesManager.ReleaseInstance(go);*/
