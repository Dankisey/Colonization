using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseInteractionArea _interactionArea;
    [SerializeField] private ResourceChecker _resourceChecker;
    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private UnitsCommander _commander;
    [SerializeField] private Unit[] _units;

    private void Awake()
    {
        foreach (Unit unit in _units)   
            unit.SetHome(this);

        _commander.SetUnits(_units);
    }

    private void OnEnable()
    {
        _resourceChecker.ResourcesFounded += OnResourceFounded;
        _interactionArea.UnitEntered += OnUnitEntered;
    }

    private void OnDisable()
    {
        _resourceChecker.ResourcesFounded -= OnResourceFounded;
        _interactionArea.UnitEntered -= OnUnitEntered;
    }

    public void Deliver(Resource resource)
    {
        _storage.Store(resource);
    }

    private void OnResourceFounded(Resource[] resources)
    {
        _commander.AddTargets(resources);
    }

    private void OnUnitEntered(Unit unit)
    {
        if (unit.HasItem == false)
            return;

        _commander.ReturnUnit(unit);
    }
}