using System.Collections.Generic;
using UnityEngine;

public class TowerGroup : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private List<TowerStatsSO> _towerStatsSO;
    [SerializeField] private List<GameObject> _towersGameObjects;

    private int _currentTower = 0;
    private GameObject _currentTowerGameObject;
    public List<TowerStatsSO> TowerStatsSO { get => _towerStatsSO; }
    public int CurrentTower { get => _currentTower; }
    public GameObject CurrentTowerGameObject { get => _currentTowerGameObject; }

    public void BuildFirstTower()
    {
        TurnOffTowers();
        _currentTower = 0;
        BuildTower(0);
        _currentTowerGameObject = _towersGameObjects[0];
    }

    public void UpdateTower()
    {
        if (CanUpdate())
        {
            TurnOffTowers();
            _currentTower++;
            BuildTower(_currentTower);
            _currentTowerGameObject = _towersGameObjects[_currentTower];
        }
    }

    public bool CanUpdate()
    {
        return (_currentTower + 1 < _towersGameObjects.Count) ;
    }

    public void UpdateTower(int index)
    {
        _currentTower = index;
    }

    public void ChangeTargetPriority(TargetPriority targetPriority)
    {
        List<TargetAquisition> targetAquisitions = new List<TargetAquisition>();
        foreach(GameObject tower in _towersGameObjects)
        {
            targetAquisitions.AddRange(tower.GetComponentsInChildren<TargetAquisition>());
        }

        foreach(TargetAquisition targetAquisition in targetAquisitions)
        {
            targetAquisition.TargetPriority = targetPriority;
        }
    }

    private void TurnOffTowers()
    {
        foreach(GameObject tower in _towersGameObjects)
        {
            tower.SetActive(false);
        }
    }

    private void BuildTower(int towerNumber)
    {
        _towersGameObjects[towerNumber].SetActive(true);
    }
}
