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


    [Header("Settings")]
    [SerializeField] private GameState _gameState = GameState.Waiting;

    private void OnEnable()
    {
        _playerHealth.OnValueChanged += GameOver;
    }

    private void OnDisable()
    {
        _playerHealth.OnValueChanged -= GameOver;
    }

    private void PlayWave()
    {
        UpdateGameState(GameState.Playing);
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
