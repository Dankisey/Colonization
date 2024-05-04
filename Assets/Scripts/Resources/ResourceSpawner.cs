using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Transform _firstBorderCorner;
    [SerializeField] private Transform _secondBorderCorner;
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private ResourceSpot _spotPrefab;

    [Header("Settings")]
    [SerializeField][Range(0f, 0.3f)] private float _randomizeOffset;
    [SerializeField] private float _baseColliderOffset;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _maxSpots;

    private readonly List<ResourceSpot> _spots = new();
    private ObjectPool<Resource> _resourcePool;
    private PositionsHolder _positionsHolder;

    private void Awake()
    {
        _positionsHolder = new(_firstBorderCorner, _secondBorderCorner, _baseColliderOffset, _randomizeOffset);
        _resourcePool = new ObjectPool<Resource>(_resourcePrefab);
        Vector3 position = new(999, 999, 999); // random position to prevent detection on spawn
        _resourcePool.SetSpawnPosition(position);
        StartCoroutine(SpawningCycle());
    }

    private bool TrySpawn()
    {
        if (TryGetSpot(out ResourceSpot spot) == false)
            return false;
        
        Resource resource = _resourcePool.Pull();
        _resourcePool.SubscribeReturnEvent(resource);
        spot.SetResource(resource);
        
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

    private bool TryGetSpot(out ResourceSpot spot)
    {
        spot = _spots.FirstOrDefault(point => point.IsOccupied == false);

        if (spot != null)        
            return true;


        if (_positionsHolder.TryGetRandomPosition(out Vector3 position) == false)
            return false;

        if (_spots.Count >= _maxSpots)
            return false;
        
        spot = Instantiate(_spotPrefab);
        spot.gameObject.transform.position = position;
        spot.Destroyed += OnSpotDestroy;
        _spots.Add(spot);

        return true;
    }

    private void OnSpotDestroy(ResourceSpot spot)
    {
        _spots.Remove(spot);
        spot.Destroyed -= OnSpotDestroy;
    }    
}