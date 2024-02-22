using NaughtyAttributes;
using Obvious.Soap;
using Unity.VisualScripting;
using UnityEngine;

public class PriorityChange : MonoBehaviour
{
    [Header("SO Dependencies")]
    [Required][SerializeField] private ScriptableEventTowerSocket _towerSocketChannel;

    private TowerSocket _currentSocket;
    private TowerGroup _currentTowerGroup;

    private void OnEnable()
    {
        _towerSocketChannel.OnRaised += UpdateSocket;
    }

    private void OnDisable()
    {
        _towerSocketChannel.OnRaised -= UpdateSocket;
    }

    public void PriorityMostHealth()
    {
        UpdatePriority(TargetPriority.MostHealth);
    }

    public void PriorityLeastHealth()
    {
        UpdatePriority(TargetPriority.LeastHealth);
    }

    public void PriorityFastest()
    {
        UpdatePriority(TargetPriority.Fastest);
    }

    public void PrioritySlowest()
    {
        UpdatePriority(TargetPriority.Slowest);
    }

    public void PriorityFurthest()
    {
        UpdatePriority(TargetPriority.Furthest);
    }

    public void PriorityClosest()
    {
        UpdatePriority(TargetPriority.Closest);
    }

    private void UpdatePriority(TargetPriority newPriority)
    {
        if (_currentSocket == null) return;
       _currentTowerGroup.ChangeTargetPriority(newPriority);
        _towerSocketChannel.Raise(_currentSocket);
    }

    private void UpdateSocket(TowerSocket socket)
    {
        _currentSocket = socket;
        _currentTowerGroup = _currentSocket.GetComponentInChildren<TowerGroup>();
    }
}
