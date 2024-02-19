using UnityEngine;

public class TargetAquisition : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _range = 10f;
    [SerializeField] private TargetPriority _targetPriority;

    private Collider[] _enemiesInRange;
    private GameObject _currentTarget;

    public GameObject CurrentTarget => _currentTarget;

    private void Update()
    {
        if(_currentTarget == null) LookForTargets();
        SetTarget();
        CheckForTargetLeaving();
    }

    private void SetTarget()
    {
          switch (_targetPriority)
        {
            case TargetPriority.Closest:
                GetClosestEnemy();
                break;
            case TargetPriority.Furthest:
                GetFurthestEnemy();
                break;
            case TargetPriority.MostHealth:
                GetEnemyWithMostHealth();
                break;
            case TargetPriority.LeastHealth:
                GetEnemyWithLeastHealth();
                break;
        }
    }

    private void LookForTargets()
    {
        _enemiesInRange = Physics.OverlapSphere(transform.position, _range, _enemyLayer);
    }

    private void GetFurthestEnemy()
    {
        float maxDistance = 0;
        foreach (var enemy in _enemiesInRange)
        {
            if(enemy.gameObject.activeInHierarchy == false) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                _currentTarget = enemy.gameObject;
            }
        }
    }

    private void GetClosestEnemy()
    {
        float minDistance = Mathf.Infinity;
        foreach (var enemy in _enemiesInRange)
        {
            if (enemy.gameObject.activeInHierarchy == false) continue;
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _currentTarget = enemy.gameObject;
            }
        }
    }

    private void GetEnemyWithMostHealth()
    {
        float maxHealth = 0;
        foreach (var enemy in _enemiesInRange)
        {
            if (enemy.gameObject.activeInHierarchy == false) continue;
            float health = enemy.GetComponent<IDamagable>().Health;
            if (health > maxHealth)
            {
                maxHealth = health;
                _currentTarget = enemy.gameObject;
            }
        }
    }

    private void GetEnemyWithLeastHealth()
    {
        float minHealth = Mathf.Infinity;
        foreach (var enemy in _enemiesInRange)
        {
            if (enemy.gameObject.activeInHierarchy == false) continue;
            float health = enemy.GetComponent<IDamagable>().Health;
            if (health < minHealth)
            {
                minHealth = health;
                _currentTarget = enemy.gameObject;
            }
        }
    }

    private void CheckForTargetLeaving()
    {
        if (_currentTarget != null)
        {
            if(Vector3.Distance(transform.position, _currentTarget.transform.position) > _range || _currentTarget.gameObject.activeInHierarchy == false)
            {
                _currentTarget = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
