using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/GameState Event Channel")]
public class GameStateChannelSO : ScriptableObject
{
    public UnityAction<Enum> GameStateEvent;

    public void RaiseEvent(Enum state)
    {
        GameStateEvent?.Invoke(state);
    }
}
