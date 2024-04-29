using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _spawner;
    [SerializeField] private Unit[] _units;

    private readonly Queue<Resource> _avaibleResources = new();
    private readonly Queue<Unit> _unitsQueue = new();
    private int _resources = 0;

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
        foreach(Unit unit in _units)
        {
            unit.SetHome(this);
            _unitsQueue.Enqueue(unit);
        }
    }

    private void OnEnable()
    {       
        _spawner.ResourceSpawned += OnResourceSpawned;
    }

    private void Start()
    {
        ResourcesAmountChanged?.Invoke(_resources);
    }

    private void OnResourceSpawned(Resource resource)
    {
        if (resource == null)
            return;

        _avaibleResources.Enqueue(resource);
        TrySetTarget();
    }

    private void OnDisable()
    {
        _spawner.ResourceSpawned -= OnResourceSpawned;
    }
}