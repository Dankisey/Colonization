using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionsHolder
{
    private readonly List<Vector3> _avaiblePositions;
    private readonly Transform _firstBorderCorner;
    private readonly Transform _secondBorderCorner;
    private readonly float _checkingOffset;
    private readonly float _randomizeOffset;

    public PositionsHolder(Transform firstCorner, Transform secondCorner, float checkingOffset, float randomizeOffset)
    {
        _firstBorderCorner = firstCorner;
        _secondBorderCorner = secondCorner;
        _checkingOffset = checkingOffset;
        _randomizeOffset = randomizeOffset;
        _avaiblePositions = Randomize(GetPositions()).ToList();
    }

    public bool TryGetRandomPosition(out Vector3 position)
    {
        bool setted = false;
        position = default;

        while(setted == false && _avaiblePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, _avaiblePositions.Count);

            if (CheckPosition(_avaiblePositions[randomIndex]))
            {
                position = _avaiblePositions[randomIndex];
                setted = true;
            }

            _avaiblePositions.RemoveAt(randomIndex);
        }

        return setted;
    }

    private bool CheckPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, _checkingOffset);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Base @base))
                return false;
        }

        return true;
    }

    private Vector3[] Randomize(Vector3[] positions)
    {
        float xOffset;
        float zOffset;

        for (int i = 0; i < positions.Length; i++)
        {
            xOffset = Random.Range(-_randomizeOffset, _randomizeOffset);
            zOffset = Random.Range(-_randomizeOffset, _randomizeOffset);

            positions[i].x += xOffset;
            positions[i].z += zOffset;
        }

        return positions;
    }

    private Vector3[] GetPositions()
    {
        Vector2Int firsBorder = new(Mathf.RoundToInt(_firstBorderCorner.position.x), Mathf.RoundToInt(_firstBorderCorner.position.z));
        Vector2Int secondBorder = new(Mathf.RoundToInt(_secondBorderCorner.position.x), Mathf.RoundToInt(_secondBorderCorner.position.z));

        int startX = Mathf.Min(firsBorder.x, secondBorder.x);
        int endX = Mathf.Max(firsBorder.x, secondBorder.x);
        int startY = Mathf.Min(firsBorder.y, secondBorder.y);
        int endY = Mathf.Max(firsBorder.y, secondBorder.y);

        int xAmount = Mathf.Abs(firsBorder.x - secondBorder.x) + 1;
        int yAmount = Mathf.Abs(firsBorder.y - secondBorder.y) + 1;
        int spotsAmount = xAmount * yAmount;
        Vector3[] positions = new Vector3[spotsAmount];

        int i = 0;

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                positions[i] = new Vector3(x, 0, y);
                i++;
            }
        }

        return positions;
    }
}