using NaughtyAttributes;
using TMPro;
using UnityEngine;
using System;
using Obvious.Soap;

public class Clock : MonoBehaviour
{
    [Header("SO Dependencies")]
    [Required][SerializeField] private ScriptableEventEnum _gameStateChannel;

    [Header("Dependencies")]
    [Required][SerializeField] private TextMeshProUGUI _timeString;

    private double _elapsedTime = 0;
    private GameState _gameState = GameState.Waiting;

    private void OnEnable()
    {
        _gameStateChannel.OnRaised += UpdateGameState;
    }

    private void OnDisable()
    {
        _gameStateChannel.OnRaised -= UpdateGameState;
    }

    private void Update()
    {
        UpdateClock();
    }

    private void UpdateClock()
    {
        if (_gameState != GameState.Playing) return;
        _elapsedTime += Time.deltaTime;
        _timeString.text = TimeSpan.FromSeconds(_elapsedTime).ToString(@"mm\:ss");
    }

    private void UpdateGameState(Enum newState)
    {
        _gameState = (GameState)newState;
    }
}
