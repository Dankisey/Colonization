using UnityEngine;

public class ResourceSpot : MonoBehaviour
{
    private Resource _resource;

    public bool IsOccupied { get; private set; } = false;

    public void SetResource(Resource resource)
    {
        IsOccupied = true;       
        resource.transform.position = transform.position;
        _resource = resource;
        _resource.ReturnPoolEvent += OnResourcePoolReturn;
    }

    private void OnResourcePoolReturn(IPoolableObject poolableObject)
    {
        poolableObject.ReturnPoolEvent -= OnResourcePoolReturn;
        IsOccupied = false;
    }
}