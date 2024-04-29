using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _spawner;
    [SerializeField] private Unit[] _units;

    private Queue<Unit> _unitsQueue;
    private Queue<Resource> _avaibleResources;
    private int _resources;

    public event Action<int> ResourcesAmountChanged;

    public void Deliver(Resource resource)
    {
        _resources++;
        ResourcesAmountChanged?.Invoke(_resources);
        resource.ReturnToPool();
    }

    private bool TrySetTarget()
    {
        if(_unitsQueue.Count == 0 || _avaibleResources.Count == 0)
            return false;

        _unitsQueue.Dequeue().SetTarget(_avaibleResources.Dequeue());

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            if (unit.HasItem == false)
                return;

            _unitsQueue.Enqueue(unit);
            unit.StoreResource();
            TrySetTarget();
        }
    }

    private void Awake()
    {
        _resources = 0;
        _avaibleResources = new Queue<Resource>();
        _unitsQueue = new Queue<Unit>();

        foreach(Unit unit in _units)
        {
            unit.SetHome(this);
            _unitsQueue.Enqueue(unit);
        }
    }

    private void OnEnable()
    {
        ResourcesAmountChanged?.Invoke(_resources);
        _spawner.ResourceSpawned += OnResourceSpawned;
    }

    private void OnResourceSpawned(Resource resource)
    {
        _avaibleResources.Enqueue(resource);
        TrySetTarget();
    }

    private void OnDisable()
    {
        _spawner.ResourceSpawned -= OnResourceSpawned;
    }
}