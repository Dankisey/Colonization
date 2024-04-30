using UnityEngine;

[RequireComponent(typeof(TargetFollower))]
public class Unit : MonoBehaviour
{
    [SerializeField] private CollectingArea _collectingArea;
    [SerializeField] private Transform _holdingPlace;

    private Base _base;
    private Resource _targetResource;
    private ItemHolder<Resource> _itemHolder;
    private TargetFollower _targetFollower;

    public bool HasItem { get; private set; } = false;

    private void Awake()
    {
        _itemHolder = new ItemHolder<Resource>(_holdingPlace);
        _targetFollower = GetComponent<TargetFollower>();
    }

    private void OnEnable()
    {
        _collectingArea.Found += OnFound;
        _itemHolder.HasItemChanged += OnHasItemChanged;
    }

    private void OnDisable()
    {
        _collectingArea.Found -= OnFound;
        _itemHolder.HasItemChanged -= OnHasItemChanged;
    }

    public void SetHome(Base home)
    {
        _base = home;
    }

    public Resource StoreResource()
    {
        if (HasItem == false)
            return null;

        Resource resource = _itemHolder.ReleaseItem();
        _base.Deliver(resource);
        _targetFollower.ResetTarget();

        return resource;
    }

    public void SetTarget(Resource target)
    {
        _targetResource = target;
        _targetFollower.SetTarget(target.transform);
    }

    private bool TryPickUp(Resource resource)
    {
        if (resource != _targetResource)
            return false;

        _itemHolder.SetItem(resource);
        _targetFollower.SetTarget(_base.transform);

        return true;
    }


    private void OnHasItemChanged(bool value)
    {
        HasItem = value;
    }

    private void OnFound(Resource resource)
    {
        TryPickUp(resource);
    }
}