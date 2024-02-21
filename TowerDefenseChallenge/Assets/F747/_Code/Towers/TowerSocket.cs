using UnityEngine;
using NaughtyAttributes;
using Lean.Pool;

public class TowerSocket : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private Transform _spawnPoint;
    [Required][SerializeField] private MeshRenderer _baseMeshRenderer;
    [Required][SerializeField] private GameObject _particles;

    private TowerGroup _towerGroup;
    private GameObject _towerGroupPrefab;
    private bool _isActiveSelected = false;
    private bool _hasTower = false;

    public TowerGroup TowerGroup { get { return _towerGroup; }}
    public bool HasTower { get { return _hasTower; } }
    public bool IsActiveSelected { get => _isActiveSelected; set { _isActiveSelected = value; ActivateParticles(value); } }

    public void BuildTower(GameObject prefab)
    {
        _towerGroupPrefab = LeanPool.Spawn(prefab, _spawnPoint);
        _towerGroup = _towerGroupPrefab.GetComponentInChildren<TowerGroup>();
        _towerGroup.BuildFirstTower();
        _hasTower = true;
        _baseMeshRenderer.enabled = false;
    }

    public void UpdateTower()
    {
        if(_towerGroup.CanUpdate())
        {
            _towerGroup.UpdateTower();
        }
    }

    public void DemolishTower()
    {
        _towerGroup.BuildFirstTower();
        _towerGroup = null;
        LeanPool.Despawn(_towerGroupPrefab);
        _hasTower = false;
        _baseMeshRenderer.enabled = true;
    }

    private void ActivateParticles(bool value)
    {
        _particles.SetActive(value);
    }
}
