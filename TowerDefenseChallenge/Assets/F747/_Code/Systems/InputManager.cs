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
        _playerInputs.PlayerInput.MainTouch.performed += ctx => OnMainTouch();
    }

    private void OnDisable()
    {
        _playerInputs.PlayerInput.LeftClick.performed -= ctx => OnLeftClick();
        _playerInputs.PlayerInput.MainTouch.performed -= ctx => OnMainTouch();
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
    
    private void OnMainTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
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
    }
}

