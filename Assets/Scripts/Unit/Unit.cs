using UnityEngine;

[RequireComponent(typeof(TargetFollower))]
public class Unit : MonoBehaviour
{
    [SerializeField] private InteractingArea _interactingArea;
    [SerializeField] private Transform _holdingPlace;

    private Base _base;
    private Resource _targetResource;
    private Flag _targetFlag;
    private ItemHolder<Resource> _itemHolder;
    private TargetFollower _targetFollower;
    private bool _isBuilder = false;

    public bool HasItem { get; private set; } = false;

    private void Awake()
    {
        _itemHolder = new ItemHolder<Resource>(_holdingPlace);
        _targetFollower = GetComponent<TargetFollower>();
    }

    private void OnEnable()
    {
        _interactingArea.BaseFound += OnBaseFound;
        _interactingArea.FlagFound += OnFlagFound;
        _interactingArea.ResourceFound += OnResourceFound;
        _itemHolder.HasItemChanged += OnHasItemChanged;
    }

    private void OnDisable()
    {
        _interactingArea.BaseFound -= OnBaseFound;
        _interactingArea.FlagFound -= OnFlagFound;
        _interactingArea.ResourceFound -= OnResourceFound;
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

    public void SetTarget(Flag flag)
    {
        _targetFlag = flag;
        _isBuilder = true;
        _targetFollower.SetTarget(flag.transform);
    }

    private bool TryPickUp(Resource resource)
    {
        if (resource != _targetResource)
            return false;

        _itemHolder.SetItem(resource);
        _targetFollower.SetTarget(_base.transform);

        return true;
    }

    private void ResetFlag()
    {
        _targetFlag = null;
        _isBuilder = false;
    }

    private void OnHasItemChanged(bool value)
    {
        HasItem = value;
    }

    private void OnBaseFound(Base @base)
    {
        @base.Enter(this);
    }

    private void OnFlagFound(Flag flag)
    {
        if (_isBuilder == false)
            return;

        if (_targetFlag == flag)
            flag.BuildBase(this);

        ResetFlag();
    }

    private void OnResourceFound(Resource resource)
    {
        TryPickUp(resource);
    }
}