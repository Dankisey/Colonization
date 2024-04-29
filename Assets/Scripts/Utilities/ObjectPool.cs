using System;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    private Queue<T> _prefabs;
    private T _prefab;

    public ObjectPool(T prefab)
    {
        _prefabs = new Queue<T>();
        _prefab = prefab;
    }

    public ObjectPool<T> CreateStartPrefabs(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            T prefab = GetNew();
            Push(prefab);
        }

        return this;
    }

    public T Pull()
    {
        if (_prefabs.Count == 0)
            return GetNew();

        T prefab = _prefabs.Dequeue();
        prefab.gameObject.SetActive(true);

        return prefab;
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        _prefabs.Enqueue(obj);
    }

    public void SubscribeReturnEvent(T parametr)
    {
        parametr.ReturnPoolEvent += OnReturnPoolEvent;
    }

    private void OnReturnPoolEvent(IPoolableObject obj)
    {
        obj.ReturnPoolEvent -= OnReturnPoolEvent;
        Push((T)obj);
    }

    private T GetNew()
    {
        return MonoBehaviour.Instantiate(_prefab);
    }
}

public interface IPoolableObject
{
    public event Action<IPoolableObject> ReturnPoolEvent;
}