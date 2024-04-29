using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CollectingArea : MonoBehaviour
{
    public event Action<Resource> Found;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Resource resource))
            Found?.Invoke(resource);
    }
}