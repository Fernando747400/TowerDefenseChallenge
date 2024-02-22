using NaughtyAttributes;
using Obvious.Soap;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("SO Dependencies")]
    [Required][SerializeField] private ScriptableEventTowerSocket _currentSocketChannel;
    [Required][SerializeField] private IntVariable _playerCurrency;
    [SerializeField] private List<Button> _priorityButtons = new List<Button>();

    [Header("Dependencies")]
    [Required][SerializeField] private GameObject _buyButton;
    [Required][SerializeField] private TextMeshProUGUI _buyText;
    [Required][SerializeField] private GameObject _updateButton;
    [Required][SerializeField] private TextMeshProUGUI _updateText;
    [Required][SerializeField] private GameObject _sellButton;
    [Required][SerializeField] private TextMeshProUGUI _sellText;
    [Required][SerializeField] private TextMeshProUGUI _priorityText;


    private TowerSocket _currentSocket;
    private TowerGroup _currentTowerGroup;

    private void OnEnable()
    {
        _currentSocketChannel.OnRaised += UpdateSocket;
        _playerCurrency.OnValueChanged += UpdateInteractablesOnCurrency;
    }

    private void OnDisable()
    {
        _currentSocketChannel.OnRaised -= UpdateSocket;
        _playerCurrency.OnValueChanged -= UpdateInteractablesOnCurrency;
    }

    private void UpdateSocket(TowerSocket newSocket)
    {
        _currentSocket = newSocket;
        if(_currentSocket == null)
        {
            HideAllButtons();
            return;
        }
        _currentTowerGroup = newSocket.GetComponentInChildren<TowerGroup>();
        UpdateButtons();
        UpdateText();
        UpdateInteractable();
        UpdateInteractablesOnCurrency(_playerCurrency.Value);
        UpdatePriorityButtons();
    }

    private void UpdateButtons()
    {
        if (!_currentSocket.HasTower)
        {
            _buyButton.SetActive(true);
            _updateButton.SetActive(false);
            _sellButton.SetActive(false);
        } else
        {
            _buyButton.SetActive(false);
            _updateButton.SetActive(true);
            _sellButton.SetActive(true);
        }
    }

    private void UpdateText()
    {
        if (!_currentSocket.HasTower) return;
   
        if (_currentTowerGroup.CanUpdate())
        {
            _updateText.text = _currentTowerGroup.TowerStatsSO[_currentTowerGroup.CurrentTower + 1].CostToPurchase.ToString();
        }
        else
        {
            _updateText.text = "Maxed";
        }

        _sellText.text = _currentTowerGroup.TowerStatsSO[_currentTowerGroup.CurrentTower].SellValue.ToString();
    }

    private void UpdateInteractable()
    {
        if(!_currentSocket.HasTower) return;
        if(_currentSocket.TowerGroup.CanUpdate())
        {
            _updateButton.GetComponentInChildren<Button>().interactable = true;
        } else
        {
            _updateButton.GetComponentInChildren<Button>().interactable = false;
        }
    }

    private void UpdateInteractablesOnCurrency(int currency)
    {
        Button buyButton = _buyButton.GetComponentInChildren<Button>();
        if(currency >= 100) //TODO chnage this bases on the selected tower in store WIP 
        {
            buyButton.interactable = true;
        } else
        {
            buyButton.interactable = false;
        }

        Button updateButton = _updateButton.GetComponentInChildren<Button>();
        if (_currentTowerGroup == null) return;
        if(_currentTowerGroup.CanUpdate() && currency >= _currentTowerGroup.TowerStatsSO[_currentTowerGroup.CurrentTower + 1].CostToPurchase)
        {
            updateButton.interactable = true;
        } else
        {
            updateButton.interactable = false;
        }
    }

    private void UpdatePriorityButtons()
    {
        foreach(Button button in _priorityButtons)
        {
            button.interactable = _currentSocket.HasTower;
        }

        if (_currentSocket.HasTower)
        {
            _priorityText.text = _currentTowerGroup.CurrentTowerGameObject.GetComponentInChildren<TargetAquisition>().TargetPriority.ToString();
        }
        else
        {
            _priorityText.text = "";
        }
    }

    private void HideAllButtons()
    {
        _updateButton.SetActive(false);
        _buyButton.SetActive(false);
        _sellButton.SetActive(false);
    }
}
