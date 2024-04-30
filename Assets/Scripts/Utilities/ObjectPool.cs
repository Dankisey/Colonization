using System;
using System.Collections.Generic;

public class ObjectPool<T> where T : IPoolableObject
{
    private readonly Queue<IPoolableObject> _prefabs;
    private readonly T _prefab;

    public ObjectPool(T prefab)
    {
        _prefabs = new Queue<IPoolableObject>();
        _prefab = prefab;
    }

    public void CreateStartPrefabs(int amount)
    {
        for (int i = 0; i < amount; i++)
            Push(GetNew());
    }

    public T Pull()
    {
        if (_prefabs.Count == 0)
            return (T)GetNew();

        IPoolableObject prefab = _prefabs.Dequeue();
        prefab.Enable();

        return (T)prefab;
    }

    public void Push(IPoolableObject obj)
    {
        obj.Disable();
        _prefabs.Enqueue(obj);
    }

    public void SubscribeReturnEvent(T obj)
    {
        obj.ReturnPoolEvent += OnReturnPoolEvent;
    }

    private void OnReturnPoolEvent(IPoolableObject obj)
    {
        obj.ReturnPoolEvent -= OnReturnPoolEvent;
        Push((T)obj);
    }

    private IPoolableObject GetNew()
    {
        return _prefab.Instantiate();
    }
}

public interface IPoolableObject
{
    public event Action<IPoolableObject> ReturnPoolEvent;

    public void Enable();

    public void Disable();

    public IPoolableObject Instantiate();
}