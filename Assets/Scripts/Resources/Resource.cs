using System;
using UnityEngine;

public class Resource : MonoBehaviour, IPoolableObject
{
    public bool IsOccupied { get; private set; } = false;

    public event Action<IPoolableObject> ReturnConditionReached;

    public bool TryOccupy(out Resource resource)
    {
        resource = null;

        if (IsOccupied)
            return false;

        IsOccupied = true;
        resource = this;

        return true;
    }

    public IPoolableObject Instantiate(Vector3 position)
    {
        return Instantiate(this, position, Quaternion.identity);
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
        ReturnConditionReached?.Invoke(this);
        IsOccupied = false;
    }

    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }
}