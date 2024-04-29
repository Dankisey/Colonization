using UnityEngine;

[RequireComponent(typeof(TargetMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private CollectingArea _collectingArea;
    [SerializeField] private Transform _holdingPlace;

    private Base _base;
    private Resource _targetResource;
    private ItemHolder<Resource> _itemHolder;
    private TargetMover _targetMover;

    public bool HasItem { get; private set; } = false;

    public void SetHome(Base home)
    {
        _base = home;
    }

    public void StoreResource()
    {
        if (HasItem == false)
            return;

        _base.Deliver(_itemHolder.ReleaseItem());
        _targetMover.ResetTarget();
    }

    public void SetTarget(Resource target)
    {
        _targetResource = target;
        _targetMover.SetTarget(target.transform);
    }

    private bool TryPickUp(Resource resource)
    {
        if (resource != _targetResource)
            return false;

        _itemHolder.SetItem(resource);
        _targetMover.SetTarget(_base.transform);

        return true;
    }

    private void Awake()
    {
        _itemHolder = new ItemHolder<Resource>(_holdingPlace);
        _targetMover = GetComponent<TargetMover>();
    }

    private void OnEnable()
    {
        _collectingArea.Found += OnFound;
        _itemHolder.HasItemChanged += OnHasItemChanged;
    }

    private void OnHasItemChanged(bool value)
    {
        HasItem = value;
    }

    private void OnFound(Resource resource)
    {
        TryPickUp(resource);
    }

    private void OnDisable()
    {
        _collectingArea.Found -= OnFound;
        _itemHolder.HasItemChanged -= OnHasItemChanged;
    }
}