using System;
using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("SO Dependencies")]
    [Required][SerializeField] private ScriptableEventEnum _gameStateChannel;
    [Required][SerializeField] private ScriptableEventInt _enemyKilledChannel;
    [Required][SerializeField] private ScriptableEventInt _enemyArrivedChannel;
    [Required][SerializeField] private ScriptableEventNoParam _startedWave;
    [Required][SerializeField] private ScriptableEventNoParam _endedWave;
    [Required][SerializeField] private IntVariable _playerHealth;
    [Required][SerializeField] private IntVariable _currentWave;
    [Required][SerializeField] private IntVariable _enemiesThisWave;

    
    [Header("Settings")]
    [SerializeField] private int _enemiesPerWaveMultiplier = 5;
    [SerializeField] private GameState _gameState = GameState.Waiting;

    private void OnEnable()
    {
        _playerHealth.OnValueChanged += GameOver;
        ResetValues();
    }

    private void OnDisable()
    {
        _playerHealth.OnValueChanged -= GameOver;
    }

    public void PlayWave()
    {
        UpdateGameState(GameState.Playing);
        _currentWave.Value++;
        _enemiesThisWave.Value = _currentWave.Value * _enemiesPerWaveMultiplier;
        _startedWave.Raise();
    }

    private void EndWave()
    {
        UpdateGameState(GameState.Waiting);
    }

    private void GameOver(int health)
    {
       if(health > 0) return;
       UpdateGameState(GameState.GameOver);
    }
  
    private void UpdateGameState(Enum newState)
    {
        if(_gameState == (GameState)newState) return;
        _gameState = (GameState)newState;
        _gameStateChannel.Raise(_gameState);
    }

    private void ResetValues()
    {
        _currentWave.Value = 0;
        _enemiesThisWave.Value = 0;
    }

    //====================================================================================================================//
    //For testing only
    [Button]
    private void StartGame()
    {
        UpdateGameState(GameState.Waiting);
    }

    [Button]
    private void StartWave()
    {
        UpdateGameState(GameState.Playing);
    }

    [Button]
    private void EndGame()
    {
        UpdateGameState(GameState.GameOver);
    }
}
