using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class InteractingArea : MonoBehaviour
{
    public event Action<Base> BaseFound;
    public event Action<Flag> FlagFound;
    public event Action<Resource> ResourceFound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
            ResourceFound?.Invoke(resource);
        else if (other.TryGetComponent(out Flag flag))
            FlagFound?.Invoke(flag);
        else if (other.TryGetComponent(out Base @base))
            BaseFound?.Invoke(@base);
    }
}