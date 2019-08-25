using System;
using UnityEngine;

namespace Source.Player
{
    public class PlayerHealth : Health
    {
        public static event Action dead;
        public static event Action<float, float> healthChanged;

        protected override void OnHealthChanged()
        {
            base.OnHealthChanged();
            healthChanged?.Invoke(Value, _maxHealth);
        }

        [ContextMenu("Die")]
        protected override void Die()
        {
            base.Die();
            dead?.Invoke();
        }

        protected override void Start()
        {
            base.Start();
            healthChanged?.Invoke(Value, _maxHealth);
        }
    }
}