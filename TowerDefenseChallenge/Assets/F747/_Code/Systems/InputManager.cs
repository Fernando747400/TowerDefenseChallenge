using UnityEngine;
using NaughtyAttributes;
using Obvious.Soap;

public class InputManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private ScriptableEventTowerSocket _towerSocketChannel;
    [SerializeField] private LayerMask _towersLayerMask;

    private MainActions _playerInputs;

    private void Awake()
    {
        _playerInputs = new MainActions();
        _playerInputs.Enable();
    }
    private void OnEnable()
    {
        _playerInputs.PlayerInput.LeftClick.performed += ctx => OnLeftClick();
    }

    private void OnDisable()
    {
        _playerInputs.PlayerInput.LeftClick.performed -= ctx => OnLeftClick();
    }

    private void OnLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _towersLayerMask))
        {
            if (hit.collider.TryGetComponent(out TowerSocket tower))
            {
                _towerSocketChannel.Raise(tower);
            }
        }
    }
}

