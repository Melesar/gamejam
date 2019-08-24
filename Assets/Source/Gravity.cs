
using System;
using UnityEngine;

[CreateAssetMenu]
public class Gravity : ScriptableObject
{
    [SerializeField] private Vector3 _value = new Vector3(0f, -10f, 0f);
    [SerializeField] private float _multiplier = 7f;

    public Vector3 Value { get; private set; }

    public Vector3 Multiplied => Value * _multiplier;

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
