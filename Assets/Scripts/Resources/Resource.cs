using System;
using UnityEngine;

public class Resource : MonoBehaviour, IPoolableObject
{
    public event Action<IPoolableObject> ReturnPoolEvent;

    public IPoolableObject Instantiate()
    {
        return Instantiate(this);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void ReturnToPool()
    {
        ReturnPoolEvent?.Invoke(this);
    }
}