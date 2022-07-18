/*--------------------------------------
Author: Quan Nguyen
+---------------------------------------
Last modified by: Quan Nguyen
--------------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
{
    public ObjectPool(T pooledObject, int numToSpawn = 0)
    {
        prefab = pooledObject;
        Spawn(numToSpawn);
    }

    public ObjectPool(T pooledObject, Action<T> pullObject, Action<T> pushObject, int numToSpawn = 0)
    {
        prefab = pooledObject;
        this.pullObject = pullObject;
        this.pushObject = pushObject;
        Spawn(numToSpawn);
    }

    private Action<T> pullObject;
    private Action<T> pushObject;
    private Queue<T> pooledObjects = new Queue<T>();
    private T prefab;
    public int pooledCount => pooledObjects.Count;

    public T Pull(Transform parent = null)
    {
        T t;
        if (pooledCount > 0)
            t = pooledObjects.Dequeue();
        else
            t = parent ? GameObject.Instantiate(prefab, parent) : GameObject.Instantiate(prefab);

        t.gameObject.SetActive(true); //ensure the object is on
        t.Initialize(Push);

        //allow default behavior and turning object back on
        pullObject?.Invoke(t);

        return t;
    }

    public T Pull(Vector3 position)
    {
        T t = Pull();
        t.transform.position = position;
        return t;
    }

    public T Pull(Vector3 position, Quaternion rotation)
    {
        T t = Pull();
        t.transform.position = position;
        t.transform.rotation = rotation;
        return t;
    }

    public GameObject PullGameObject()
    {
        return Pull().gameObject;
    }

    public GameObject PullGameObject(Vector3 position)
    {
        GameObject go = Pull().gameObject;
        go.transform.position = position;
        return go;
    }

    public GameObject PullGameObject(Vector3 position, Quaternion rotation)
    {
        GameObject go = Pull().gameObject;
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }

    public void Push(T t)
    {
        pooledObjects.Enqueue(t);

        //create default behavior to turn off objects
        pushObject?.Invoke(t);

        t.gameObject.SetActive(false);
    }

    private void Spawn(int number)
    {
        T t;

        for (int i = 0; i < number; i++)
        {
            t = GameObject.Instantiate(prefab);
            pooledObjects.Enqueue(t);
            t.gameObject.SetActive(false);
        }
    }
}

public interface IPool<T>
{
    T Pull(Transform parent = null);
    void Push(T t);
}

public interface IPoolable<T>
{
    void Initialize(Action<T> returnAction);
    void ReturnToPool();
}