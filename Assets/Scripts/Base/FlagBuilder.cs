using UnityEngine;

public class FlagBuilder : MonoBehaviour
{
    private Base _selectedBase = null;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
             DoInteraction();
        }
    }

    private void DoInteraction()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1;

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent(out Base @base))
                _selectedBase = @base;

            if(hit.collider.TryGetComponent(out Floor floor))
            {
                if (_selectedBase == null)
                    return;

                Flag flag = _selectedBase.GetFlag();
                flag.transform.position = hit.point;
            }
        }
    }
}