using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New FloatVariable", menuName = "SharedVariables/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    [SerializeField] private float _initialValue;
    [NonSerialized] private float _value;

    public event Action<float> OnValueChanged = delegate { };

    private void OnEnable() => _value = _initialValue;

    public float Value
    {
        get => _value;
        set
        {
            if (Mathf.Approximately(value, _initialValue)) return;
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }
}
