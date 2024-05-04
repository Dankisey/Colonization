using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private ResourceStorage _storage;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Unit _prefab;
    [SerializeField] private int _unitCost = 3;

    public bool TrySpawn(out Unit unit)
    {
        unit = null;

        if (_storage.TrySpend(_unitCost) == false)
            return false;
            
        unit = Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);

        return true;
    }
}