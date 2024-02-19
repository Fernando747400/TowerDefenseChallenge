using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class ProyectileShooter : MonoBehaviour
{
    [Header("Dependencies")]
    [Required]
    [SerializeField] private TargetAquisition _targetAquisition;
    [Required]
    [SerializeField] private Transform _proyectileSpawnPoint;
    [Required]
    [SerializeField] private GameObject _proyectilePrefab;

    [Header("Settings")]
    [SerializeField] private float _fireRate = 1f;

    private float _fireRateTimer = 0f;

    private GameObject _currentTarget;

    private void Update()
    {
        _fireRateTimer += Time.deltaTime;

        if (_fireRateTimer >= _fireRate)
        {
            GetCurrentTarget();
            ShootProyectile();
            _fireRateTimer = 0;
        }
    }

    private void GetCurrentTarget()
    {
        _currentTarget = _targetAquisition.CurrentTarget ?? null;
    }

    public void ShootProyectile()
    {
        if (_currentTarget == null) return;
          GameObject proyectile = LeanPool.Spawn(_proyectilePrefab, _proyectileSpawnPoint.position, _proyectileSpawnPoint.rotation);
          proyectile.GetComponentInChildren<BasicProyectile>().Target = _targetAquisition.CurrentTarget;
     }
}
