
using System;
using UnityEngine;

[CreateAssetMenu]
public class Gravity : ScriptableObject
{
    [SerializeField] private float _value;

    public float Value { get; private set; }

    public event Action onGravitySwitched;
    
    public void Switch()
    {
        Value = -Value;
        onGravitySwitched?.Invoke();
    }

    public void ResetValue()
    {
        Value = _value;
    }

    private void OnEnable()
    {
        Value = _value;
    }
}
