using Lean.Pool;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private List<WayPointManager> _wayPointManagers;
    [SerializeField] private List<EnemyStatsSO> _enemyStats;

    [Button]
    public void SpawnEnemy()
    {
        HandleSpawn(GetRandomEnemyStats(), GetRandomWaypointManager());
    }

    private void HandleSpawn(EnemyStatsSO enemyStats, WayPointManager wayPointManager)
    {
        GameObject newSpawn = LeanPool.Spawn(enemyStats.Prefab, transform.position, Quaternion.identity);
        newSpawn.transform.localScale = enemyStats.ModelScale;
        SetUpWaypointFollower(newSpawn, enemyStats, wayPointManager);
        SetUpEnemyStats(newSpawn, enemyStats);
    }

    private void SetUpWaypointFollower(GameObject newSpawn, EnemyStatsSO enemyStats, WayPointManager wayPointManager)
    {
        WayPointFollower wayPointFollower = newSpawn.GetComponentInChildren<WayPointFollower>();
        wayPointFollower.MoveSpeed = enemyStats.MoveSpeed;
        wayPointFollower.RotationForce = enemyStats.RotationSpeed;
        wayPointFollower.ArriveDistance = enemyStats.ArriveDistance;
        wayPointFollower.WaypointsToFollow = wayPointManager;
        wayPointFollower.Spawn();
    }
    private void SetUpEnemyStats(GameObject newSpawn, EnemyStatsSO enemyStats)
    {
        BaseEnemy baseEnemy = newSpawn.GetComponentInChildren<BaseEnemy>();
        baseEnemy.Health = enemyStats.MaxHealth;
        baseEnemy.ValueOnDeath = enemyStats.CoinsOnDeath;
        baseEnemy.DamageToPlayer = enemyStats.DamageToPlayer;
    }

    private WayPointManager GetRandomWaypointManager()
    {
        return _wayPointManagers[Random.Range(0, _wayPointManagers.Count)];
    }

    private EnemyStatsSO GetRandomEnemyStats()
    {
        return _enemyStats[Random.Range(0, _enemyStats.Count)];
    }
}
