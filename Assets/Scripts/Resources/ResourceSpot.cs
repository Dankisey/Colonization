using System;
using UnityEngine;

public class ResourceSpot : MonoBehaviour
{
    private Resource _resource;

    public bool IsOccupied { get; private set; } = false;

    public event Action<ResourceSpot> Destroyed;

    private void OnDestroy()
    {
        if ( _resource != null )
            _resource.ReturnConditionReached -= OnReturnConditionReached;

        Destroyed?.Invoke(this);
    }

    public void SetResource(Resource resource)
    {
        IsOccupied = true;       
        resource.transform.position = transform.position;
        _resource = resource;
        _resource.ReturnConditionReached += OnReturnConditionReached;
    }

    private void OnReturnConditionReached(IPoolableObject poolableObject)
    {
        _resource.ReturnConditionReached -= OnReturnConditionReached;
        _resource = null;
        IsOccupied = false;
    }
}