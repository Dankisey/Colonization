using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsCommander : MonoBehaviour
{
    private readonly HashSet<Resource> _avaibleTargets = new();
    private readonly List<Resource> _currentTargets = new();
    private readonly Queue<Unit> _avaibleUnits = new();

    private bool _IsBuildingBase = false;
    private Flag _flag = null;

    public void AddUnit(Unit unit) 
    {
        _avaibleUnits.Enqueue(unit);
    }

    public void BuildBase(Flag flag)
    {
        _flag = flag;
        _IsBuildingBase = true;
    }

    public void ReturnUnit(Unit unit)
    {
        Resource resource = unit.StoreResource();
        _avaibleUnits.Enqueue(unit);
        _avaibleTargets.Remove(resource);
        _currentTargets.Remove(resource);

        if (_IsBuildingBase)
            SendBuilder();
        else
            TrySetTarget();
    }

    public void AddTargets(Resource[] resources)
    {
        foreach (Resource resource in resources)
            _avaibleTargets.Add(resource);

        TrySetTarget();
    }

    private void SendBuilder()
    {
        _avaibleUnits.Dequeue().SetTarget(_flag);
        _IsBuildingBase = false;
        _flag = null;
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