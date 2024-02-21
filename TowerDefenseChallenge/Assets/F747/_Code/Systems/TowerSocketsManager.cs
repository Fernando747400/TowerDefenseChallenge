using UnityEngine;
using NaughtyAttributes;
using Obvious.Soap;

public class TowerSocketsManager : MonoBehaviour
{
    [Header("SO Dependencies")]
    [Required][SerializeField] private ScriptableEventTowerSocket _towerSocketChannel;

    private TowerSocket _currentSocket;

    private void OnEnable()
    {
        _towerSocketChannel.OnRaised += OnTowerSocketRaised;
    }

    private void OnDisable()
    {
        _towerSocketChannel.OnRaised -= OnTowerSocketRaised;
    }

    private void OnTowerSocketRaised(TowerSocket towerSocket)
    {
        ReleaseLastSocket();
        HandleNewSocket(towerSocket);
    }

    private void ReleaseLastSocket()
    {
        if(_currentSocket == null) return;

        _currentSocket.IsActiveSelected = false;
    }

    private void HandleNewSocket(TowerSocket towerSocket)
    {
        _currentSocket = towerSocket;
        towerSocket.IsActiveSelected = true;
    }
}
