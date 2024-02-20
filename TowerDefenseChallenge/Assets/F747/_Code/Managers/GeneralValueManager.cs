using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class GeneralValueManager : MonoBehaviour
{
    [Header("SO Dependencies")]
    [Required][SerializeField] private ScriptableEventInt _intValueChannel;
    [Required][SerializeField] private IntVariable _intVariable;

    [Header("Settings")]
    [Tooltip("If this is true, it will modify the value as +=")]
    [SerializeField] private bool _isComulative;

    private void OnEnable()
    {
        _intValueChannel.OnRaised += ChangeValue;
    }

    private void OnDisable()
    {
        _intValueChannel.OnRaised -= ChangeValue;
    }

    private void ChangeValue(int newValue)
    {
        if(_isComulative)
        {
            _intVariable.Value += newValue;
        }
        else
        {
            _intVariable.Value = newValue;
        }
    }
}
