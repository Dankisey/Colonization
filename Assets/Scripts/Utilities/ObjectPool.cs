using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : IPoolableObject
{
    private readonly Queue<T> _prefabs;
    private readonly T _prefab;
    private Vector3 _spawnPosition;

    public ObjectPool(T prefab)
    {
        _spawnPosition = Vector3.zero;
        _prefabs = new Queue<T>();
        _prefab = prefab;
    }

    public void CreateStartPrefabs(int amount)
    {
        for (int i = 0; i < amount; i++)
            Push(GetNew());
    }

    public void SetSpawnPosition(Vector3 position)
    {
        _spawnPosition = position;
    }

    public T Pull()
    {
        if (_prefabs.Count == 0)
        {
            return GetNew();
        }
        else
        {
            T obj = _prefabs.Dequeue();
            obj.Enable();

            return obj;
        }
    }

    public void Push(T obj)
    {
        obj.Disable();
        obj.SetPosition(_spawnPosition);
        _prefabs.Enqueue(obj);
    }

    public void SubscribeReturnEvent(T obj)
    {
        obj.ReturnConditionReached += OnReturnConditionReached;
    }

    private void OnReturnConditionReached(IPoolableObject obj)
    {
        obj.ReturnConditionReached -= OnReturnConditionReached;
        Push((T)obj);
    }

    private T GetNew()
    {
        return (T)_prefab.Instantiate(_spawnPosition);
    }
}

public interface IPoolableObject
{
    public event Action<IPoolableObject> ReturnConditionReached;

    public void SetPosition(Vector3 position);

    public void Enable();

    public void Disable();

    public IPoolableObject Instantiate(Vector3 position);
}