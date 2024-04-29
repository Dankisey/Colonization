using System;
using UnityEngine;

public class Resource : MonoBehaviour, IPoolableObject
{
    public event Action<IPoolableObject> ReturnPoolEvent;

    public void ReturnToPool()
    {
        ReturnPoolEvent?.Invoke(this);
    }
}