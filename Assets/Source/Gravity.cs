
using System;
using UnityEngine;

public static class Gravity
{
    public static Vector3 Value { get; private set; } = new Vector3(0f, -1f, 0f);

    public static event Action OnGravitySwitched;
    
    public static void Switch()
    {
        Value = -Value;
        OnGravitySwitched?.Invoke();
    }
}
