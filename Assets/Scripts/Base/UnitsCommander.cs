using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsCommander : MonoBehaviour
{
    private readonly HashSet<Resource> _avaibleTargets = new();
    private readonly List<Resource> _currentTargets = new();
    private readonly Queue<Unit> _avaibleUnits = new();

    public void SetUnits(Unit[] units) 
    {
        foreach (Unit unit in units)
            _avaibleUnits.Enqueue(unit);
    }

    public void ReturnUnit(Unit unit)
    {
        Resource resource = unit.StoreResource();
        _avaibleUnits.Enqueue(unit);
        _avaibleTargets.Remove(resource);
        _currentTargets.Remove(resource);
        TrySetTarget();
    }

    public void AddTargets(Resource[] resources)
    {
        foreach (Resource resource in resources)
            _avaibleTargets.Add(resource);

        TrySetTarget();
    }

    private bool TrySetTarget()
    {
        if (_avaibleUnits.Count == 0 || _avaibleTargets.Count - _currentTargets.Count == 0)
            return false;

        Resource target = _avaibleTargets.Except(_currentTargets).FirstOrDefault();
        _currentTargets.Add(target);
        _avaibleUnits.Dequeue().SetTarget(target);

        return true;
    }
}