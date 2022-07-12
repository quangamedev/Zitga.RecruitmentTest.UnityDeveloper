/*--------------------------------------
Author: Quan Nguyen
+---------------------------------------
Last modified by: Quan Nguyen
--------------------------------------*/

using System;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolable<PoolObject>
{
    private Action<PoolObject> returnToPool;

    private void OnDisable()
    {
        ReturnToPool();
    }

    public void Initialize(Action<PoolObject> returnAction)
    {
        //cache reference to return action
        returnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        //invoke and return this object to pool
        returnToPool?.Invoke(this);
    }
}