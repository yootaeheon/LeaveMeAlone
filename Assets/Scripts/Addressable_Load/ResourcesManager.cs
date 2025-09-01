using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ResourcesManager
{
    /// <summary>
    /// 어드레서블 에셋을 비동기로 로드 (T 타입 반환)
    /// </summary>
    public static async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken ct = default) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        await handle.ToUniTask(cancellationToken: ct);
        if (handle.Status == AsyncOperationStatus.Succeeded)
            return handle.Result;
        Debug.LogError($"[ResourcesManager] 에셋 로드 실패: {key}");
        return null;
    }

    /// <summary>
    /// 어드레서블 프리팹을 비동기로 로드 후 인스턴스화 (GameObject 반환)
    /// </summary>
    public static async UniTask<GameObject> InstantiateAsync(string key, Transform parent = null, bool worldPositionStays = true, CancellationToken ct = default)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, parent, worldPositionStays);
        await handle.ToUniTask(cancellationToken: ct);
        if (handle.Status == AsyncOperationStatus.Succeeded)
            return handle.Result;
        Debug.LogError($"[ResourcesManager] 인스턴스화 실패: {key}");
        return null;
    }

    /// <summary>
    /// 어드레서블 에셋 해제
    /// </summary>
    public static void Release<T>(T obj) where T : UnityEngine.Object
    {
        Addressables.Release(obj);
    }

    /// <summary>
    /// 어드레서블 인스턴스 해제
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
