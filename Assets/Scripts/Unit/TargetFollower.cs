using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TargetFollower : MonoBehaviour
{
    [SerializeField][Range(0, 1)] private float _offset;
    [SerializeField] private float _speed;

    private CharacterController _characterController;
    private Transform _transform;
    private Transform _target;

    public void ResetTarget()
    {
        _target = null;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _transform = transform;
    }

    private void Update()
    {
        if (_target == null)
            return;

        Vector3 direction = _target.position - _transform.position;
        direction.y = 0;
        Vector3 movement = (direction.normalized * _speed + Physics.gravity) * Time.deltaTime;

        _characterController.Move(movement);

        if ((_target.position - _transform.position).magnitude <= _offset)
            ResetTarget();
    }
}