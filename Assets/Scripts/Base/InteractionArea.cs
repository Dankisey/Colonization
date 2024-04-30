using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class BaseInteractionArea : MonoBehaviour
{
    public event Action<Unit> UnitEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
            UnitEntered?.Invoke(unit);
    }
}