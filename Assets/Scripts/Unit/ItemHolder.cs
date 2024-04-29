using System;
using UnityEngine;

public class ItemHolder<T> where T : MonoBehaviour
{
    private readonly Transform _holdingPlace;
    private T _holdingItem;

    public event Action<bool> HasItemChanged;

    public ItemHolder(Transform holdingPlace)
    {
        _holdingPlace = holdingPlace;
    }

    public void SetItem(T item)
    {
        _holdingItem = item;
        _holdingItem.transform.SetParent(_holdingPlace.transform);
        _holdingItem.transform.localPosition = Vector3.zero;
        HasItemChanged?.Invoke(true);
    }

    public T ReleaseItem()
    {
        _holdingItem.transform.SetParent(null);
        HasItemChanged?.Invoke(false);

        return _holdingItem;
    }
}