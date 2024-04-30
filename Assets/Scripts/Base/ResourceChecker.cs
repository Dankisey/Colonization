using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceChecker : MonoBehaviour
{
    [SerializeField] private float _searchingRadius;
    [SerializeField] private float _searchInterval;

    private readonly List<Resource> _resources = new();

    public event Action<Resource[]> ResourcesFounded;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _searchingRadius);
    }

    private void Start()
    {
        StartCoroutine(SearchingCycle());
    }

    private IEnumerator SearchingCycle()
    {
        var wait = new WaitForSeconds(_searchInterval);

        while (true)
        {
            _resources.Clear();

            Collider[] colliders = Physics.OverlapSphere(transform.position, _searchingRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Resource resource))
                    _resources.Add(resource);
            }

            if(_resources.Count > 0)
                ResourcesFounded?.Invoke(_resources.ToArray());

            yield return wait;
        }
    }
}