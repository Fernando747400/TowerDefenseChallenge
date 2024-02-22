using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Obvious.Soap;

public class StoreManager : MonoBehaviour
{
    [Header("SO Dependencies")]
    [Required][SerializeField] private ScriptableEventTowerSocket _currentSocketChannel;
    [Required][SerializeField] private IntVariable _playerCurrency;

    [SerializeField] private GameObject testPrefab;

    [SerializeField] private List<GameObject> _towersPrefabs;

    private TowerSocket _currentSocket;

    private void OnEnable()
    {
        _currentSocketChannel.OnRaised += UpdateTowerSocket;
    }

    private void OnDisable()
    {
        _currentSocketChannel.OnRaised -= UpdateTowerSocket;
    }

    public void BuyFirstTower(GameObject tower)
    {
        if (CheckNullSocket()) return;
        TowerGroup towerGroup = tower.GetComponentInChildren<TowerGroup>();
        int currentPrice = towerGroup.TowerStatsSO[towerGroup.CurrentTower].CostToPurchase;
        if(_playerCurrency.Value >= currentPrice)
        {
            _playerCurrency.Value -= currentPrice;
            _currentSocket.BuildTower(tower);
        }
        _currentSocketChannel.Raise(_currentSocket);
    }

    [Button]
    public void UpdateTower()
    {
        if(CheckNullSocket()) return;

        TowerGroup towerGroup = _currentSocket.GetComponentInChildren<TowerGroup>();

        if (!towerGroup.CanUpdate()) return;

        int currentPrice = towerGroup.TowerStatsSO[towerGroup.CurrentTower + 1].CostToPurchase;
        if(_playerCurrency.Value >= currentPrice)
        {
            _playerCurrency.Value -= currentPrice;
            _currentSocket.UpdateTower();
        }
        _currentSocketChannel.Raise(_currentSocket);
    }

    [Button]
    public void SellTower()
    {
        if(CheckNullSocket()) return;

        TowerGroup towerGroup = _currentSocket.GetComponentInChildren<TowerGroup>();

        _playerCurrency.Value += towerGroup.TowerStatsSO[towerGroup.CurrentTower].SellValue;
        _currentSocket.DemolishTower();
        _currentSocketChannel.Raise(_currentSocket);
    }

    private void UpdateTowerSocket(TowerSocket newTower)
    {
        _currentSocket = newTower;
    }

    private bool CheckNullSocket()
    {
        return ( _currentSocket == null );
    }

    //For testing only
    [Button]
    private void BuildBasicTower()
    {
        BuyFirstTower(testPrefab);
    }

}
