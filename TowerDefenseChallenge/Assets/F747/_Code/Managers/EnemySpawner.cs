using Lean.Pool;
using NaughtyAttributes;
using Obvious.Soap;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("SO Dependencies")]
    [Required][SerializeField] private ScriptableEventEnum _gameStateChannel;
    [Required][SerializeField] private ScriptableEventNoParam _startedWaveChannel;
    [Required][SerializeField] private ScriptableEventNoParam _endedWaveChannel;
    [Required][SerializeField] private IntVariable _currentWave;
    [Required][SerializeField] private IntVariable _enemiesThisWave;

    [Header("Dependencies")]
    [SerializeField] private List<WayPointManager> _wayPointManagers;
    [SerializeField] private List<EnemyStatsSO> _enemyStats;

    [Header("Settings")]
    [SerializeField] private int _spawnBigEnemiesAtWave = 10;
    [SerializeField] private float _maxBigEnemyPerWaveDivition = 6f;
    [SerializeField] private int _spawnSmallEnemiesAtWave = 5;
    [SerializeField] private float _maxSmallEnemyPerWaveDivition = 4f;
    [Range(0.3f, 10f)]
    [SerializeField] private float _spawnInterval = 1f;


    private GameState _gameState = GameState.Waiting;
    private float _elapsedTime = 0;
    private float _elapsedRandomTime = 0;
    private int _currentEnemiesSpawned = 0;
    private bool _canSpawn;
    private int _currentBigEnemiesSpawned = 0;
    private int _currentSmallEnemiesSpawned = 0;
    private int _maxBigEnemyThisWave;
    private int _maxSmallEnemyThisWave;


    private void OnEnable()
    {
        _gameStateChannel.OnRaised += UpdateGameState;
        _startedWaveChannel.OnRaised += SpawnWave;
    }

    private void OnDisable()
    {
        _gameStateChannel.OnRaised -= UpdateGameState;
        _startedWaveChannel.OnRaised -= SpawnWave;
    }

    public void Update()
    {
     _elapsedTime += Time.deltaTime;
     SpawnEnemyInWave();
    }

    [Button]
    private void SpawnEnemy()
    {
        if (_gameState != GameState.Playing) return;
        if(_currentEnemiesSpawned < _enemiesThisWave)
        {
            HandleSpawn(GetRandomEnemyStats(), GetRandomWaypointManager());
            _currentEnemiesSpawned++;
            if (_currentBigEnemiesSpawned == _enemiesThisWave) _endedWaveChannel.Raise();
        }
        else
        {
            _canSpawn = false;
        }
    }

    private void SpawnWave()
    {
        CalculateSpecialEnemiesMax();
        _currentEnemiesSpawned = 0;
        _canSpawn = true;
    }

    private void HandleSpawn(EnemyStatsSO enemyStats, WayPointManager wayPointManager)
    {
        GameObject newSpawn = LeanPool.Spawn(enemyStats.Prefab, transform.position, Quaternion.identity);
        newSpawn.transform.localScale = enemyStats.ModelScale;
        SetUpEnemyStats(newSpawn, enemyStats);
        SetUpWaypointFollower(newSpawn, enemyStats, wayPointManager);
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
        return _wayPointManagers[UnityEngine.Random.Range(0, _wayPointManagers.Count)];
    }

    private EnemyStatsSO GetRandomEnemyStats()
    {
        int randomEnemyIndex = UnityEngine.Random.Range(0, 3);

        switch (randomEnemyIndex)
        {
            case 0:
                return _enemyStats[randomEnemyIndex];

            case 1 when CanSpawnSmallEnemy():
                _currentSmallEnemiesSpawned++;
                return _enemyStats[randomEnemyIndex];

            case 2 when CanSpawnBigEnemy():
                _currentBigEnemiesSpawned++;
                return _enemyStats[randomEnemyIndex];

            default:
                return _enemyStats[0];
        }
    }

    private bool CanSpawnSmallEnemy()
    {
        return _currentWave >= _spawnSmallEnemiesAtWave && _currentSmallEnemiesSpawned < _maxSmallEnemyThisWave;
    }

    private bool CanSpawnBigEnemy()
    {
        return _currentWave >= _spawnBigEnemiesAtWave && _currentBigEnemiesSpawned < _maxBigEnemyThisWave;
    }

    private void CalculateSpecialEnemiesMax()
    {
        _currentBigEnemiesSpawned = 0;
        _currentSmallEnemiesSpawned = 0;
        _maxBigEnemyThisWave = Mathf.RoundToInt( _currentWave/ _maxBigEnemyPerWaveDivition);
        _maxSmallEnemyThisWave = Mathf.RoundToInt(_currentWave / _maxSmallEnemyPerWaveDivition);
    }

    private void UpdateGameState(Enum newState)
    {
        _gameState = (GameState)newState;
    }

    private void SpawnEnemyInWave()
    {
        if(_elapsedTime > _elapsedRandomTime && _canSpawn)
        {
            RandomInterval();
            _elapsedTime = 0;
            SpawnEnemy();
        }
    }

    private void RandomInterval()
    {
        _elapsedRandomTime = _spawnInterval * UnityEngine.Random.Range(0.7f, 1.3f);
    }
}
