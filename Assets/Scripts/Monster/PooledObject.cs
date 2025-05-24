using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [HideInInspector] public ObjectPool _returnPool;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _returnPool = GetComponentInParent<ObjectPool>();
    }

    public void CallReturnPool()
    {
        _returnPool.RetrunPool(this);
    }
}
