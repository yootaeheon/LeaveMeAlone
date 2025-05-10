using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public ObjectPool _returnPool;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _returnPool.RetrunPool(this);
        }
    }
}
