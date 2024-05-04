using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private float _clearRadius = 5;
    [SerializeField] private Base _prefab;

    private Base _parent;

    public event Action BaseBuilded;

    public void SetParent(Base parent)
    {
        _parent = parent;
    }

    public void BuildBase(Unit unit)
    {
        ClearTerritory();
        Base child = Instantiate(_prefab, transform.position, Quaternion.identity);

        _parent.TryRemoveUnit(unit);
        child.TryAddUnit(unit);

        BaseBuilded?.Invoke();

        Destroy(gameObject);
    }

    private void ClearTerritory()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _clearRadius);

        foreach (Collider collider in colliders) 
        {
            if (collider.TryGetComponent(out Resource resource))
                resource.ReturnToPool();

            if (collider.TryGetComponent(out ResourceSpot spot))
                Destroy(spot.gameObject);

        }
    }
}