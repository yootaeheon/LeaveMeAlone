using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀; 오브젝를 미리 생성해놓고 활성화/비활성화 방식으로 관리;
/// 메모리 외부 단편화 방지;
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [System.NonSerialized]
    public List<PooledObject> Pool = new List<PooledObject>();

    public List<PooledObject> ForResetPosList;

    [SerializeField] PooledObject _prefab;

    [SerializeField] int _size;

    [SerializeField] Transform _poolTransform;

    private void Awake()
    {
        for (int i = 0; i < _size; i++)
        {
            PooledObject instance = Instantiate(_prefab, _poolTransform.position, Quaternion.identity);
            instance.gameObject.SetActive(false);
            instance._returnPool = this;
            instance.transform.SetParent(_poolTransform, false);
            Pool.Add(instance);
            ForResetPosList.Add(instance);
        }
    } 

    public PooledObject GetPool(Vector3 position, Quaternion rotation)
    {
        if (Pool.Count > 0)
        {
            PooledObject instance = Pool[Pool.Count - 1];
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.transform.localScale = new Vector3(2,2,2);
            instance.gameObject.SetActive(true);

            Pool.RemoveAt(Pool.Count - 1);

            return instance;
        }
        else
        {
            PooledObject instance = Instantiate(_prefab, position, rotation);
            instance.transform.SetParent(_poolTransform, false);
            instance._returnPool = this;
            Pool.Add(instance);
            return instance;
        }
    }

    public void RetrunPool(PooledObject instance)
    {
        instance.gameObject.SetActive(false);
        /* instance.transform.parent = _poolTransform;*/
        instance.transform.position = _poolTransform.transform.position;
        Pool.Add(instance);
    }

    public void ResetPos()
    {
        foreach (PooledObject obj in ForResetPosList)
        {
            obj.transform.position = _poolTransform.transform.position;
        }
    }

}
