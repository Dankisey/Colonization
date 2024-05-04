using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceChecker _resourceChecker;
    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private UnitsCommander _commander;
    [SerializeField] private UnitSpawner _spawner;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private int _baseCost = 5;

    private Flag _currentFlag = null;

    private void Awake()
    {
        foreach (Unit unit in _units)
        {
            if(unit == null) 
                continue;

            unit.SetHome(this);
            _commander.AddUnit(unit);
        }
    }

    private void OnEnable()
    {
        _resourceChecker.ResourcesFounded += OnResourceFounded;
    }

    private void OnDisable()
    {
        _resourceChecker.ResourcesFounded -= OnResourceFounded;
    }

    public Flag GetFlag()
    {
        if (_currentFlag == null)
        {
            _currentFlag = Instantiate(_flagPrefab);
            _currentFlag.SetParent(this);
            _currentFlag.BaseBuilded += OnBaseBuilded;
        }

        return _currentFlag;
    }

    public bool TryAddUnit(Unit unit)
    {
        if (_units.Contains(unit))
            return false;

        unit.SetHome(this);
        _units.Add(unit);
        _commander.AddUnit(unit);

        return true;
    }

    public bool TryRemoveUnit(Unit unit)
    {
        return _units.Remove(unit);
    }

    public void Enter(Unit unit)
    {
        if (_units.Contains(unit) == false || unit.HasItem == false)
            return;

        _commander.ReturnUnit(unit);
    }

    public void Deliver(Resource resource)
    {
        _storage.Store(resource);

        if (_currentFlag == null)
            TryCreateUnit();
        else
            TryBuildBase();
    }

    private void TryBuildBase()
    {
        if (_storage.TrySpend(_baseCost))
        {
            _commander.BuildBase(_currentFlag);
        }
    }

    private void TryCreateUnit()
    {
        if (_spawner.TrySpawn(out Unit unit))
            TryAddUnit(unit);
    }

    private void OnResourceFounded(Resource[] resources)
    {
        _commander.AddTargets(resources);
    }

    private void OnBaseBuilded()
    {
        _currentFlag.BaseBuilded -= OnBaseBuilded;
        _currentFlag = null;
    }
}