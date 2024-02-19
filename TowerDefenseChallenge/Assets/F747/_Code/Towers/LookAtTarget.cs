using NaughtyAttributes;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [Header("Dependencies")]
    [Required]
    [SerializeField] private TargetAquisition _targetAquisition;

    [Header("Settings")]
    [SerializeField] private float _rotationSpeed = 5f;

    private Transform _currentTarget;

    private void Update()
    {
       GetCurrentTarget();
       RotateTowardsTarget();
    }

    private void GetCurrentTarget()
    {
        _currentTarget = _targetAquisition.CurrentTarget?.transform ?? null;
    }

    private void RotateTowardsTarget()
    {
        if (_currentTarget == null) return;
        Vector3 directionToTarget = _currentTarget.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

}
