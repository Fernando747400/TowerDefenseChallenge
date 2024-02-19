using Lean.Pool;
using UnityEngine;

public class BasicProyectile : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Settings")]
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private GameObject _target;

   public GameObject Target
    {
        get => _target;
        set => _target = value;
    }

    private void FixedUpdate()
    {
        HomeOnTarget();
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_enemyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            other.GetComponent<IDamagable>().TakeDamage(_damage);
        }

        LeanPool.Despawn(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_enemyLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(_damage);
        }

        LeanPool.Despawn(gameObject);
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * _speed * Time.fixedDeltaTime);
    }

    private void HomeOnTarget()
    {
        if (_target != null && _target.gameObject.activeInHierarchy)
        {
            Vector3 direction = _target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
