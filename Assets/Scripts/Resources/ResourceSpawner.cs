using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _distance;

    private ObjectPool<Resource> _pool;
    private ResourceSpot[] _spots;

    public event Action<Resource> ResourceSpawned;

    private void Awake()
    {
        _spots = transform.GetComponentsInChildren<ResourceSpot>();
        _pool = new ObjectPool<Resource>(_prefab);
        _pool.CreateStartPrefabs(_spots.Length);
        DoCircle();
        StartCoroutine(SpawningCycle());
    }

    private bool TrySpawn()
    {
        ResourceSpot spot = _spots.FirstOrDefault(point => point.IsOccupied == false);

        if (spot == null)
            return false;

        Resource resource = _pool.Pull();
        _pool.SubscribeReturnEvent(resource);
        spot.SetResource(resource);
        ResourceSpawned?.Invoke(resource);

        return true;
    }

    private IEnumerator SpawningCycle()
    {
        var wait = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            yield return wait;

            TrySpawn();
        }
    }

    private void DoCircle()
    {
        int points = _spots.Length;
        float angleStep = 360 / points;
        float currentAngle = 0;
        float height = _spots[0].transform.position.y;

        for (int i = 0; i < points; i++)
        {
            Vector3 newPosition = new(Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0, Mathf.Sin(currentAngle * Mathf.Deg2Rad));
            newPosition *= _distance;
            newPosition.y = height;
            _spots[i].transform.position = newPosition;
            currentAngle += angleStep;
        }
    }
}