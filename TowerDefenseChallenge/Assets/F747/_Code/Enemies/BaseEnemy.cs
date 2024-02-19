using Lean.Pool;
using Obvious.Soap;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    [Header("SO Channels Dependenices")]
    [SerializeField] private ScriptableEventFloat _onEnemyDeath;
    [SerializeField] private ScriptableEventFloat _onEnemyArrival;

    [Header("Dependencies")]
    [SerializeField] private WayPointFollower _waypointFollower;

    private float _health = 100f;
    private float _damageToPlayer = 1f;
    private float _valueOnDeath = 1f;
    public float Health { get => _health; set => _health = value; }
    public float DamageToPlayer { get => _damageToPlayer; set => _damageToPlayer = value; }
    public float ValueOnDeath { get => _valueOnDeath; set => _valueOnDeath = value; }

    private void OnEnable()
    {
        _waypointFollower.OnArriveToLastCheckpoint.AddListener(Arrived);
    }

    private void OnDisable()
    {
        _waypointFollower.OnArriveToLastCheckpoint.RemoveListener(Arrived);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if(_health <= 0)
        {
            _onEnemyDeath.Raise(_valueOnDeath);
            LeanPool.Despawn(gameObject); //Change this to pooling
        }
    }

    private void Arrived()
    {
        _onEnemyArrival.Raise(_damageToPlayer);
    }
}
