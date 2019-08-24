using System;
using UnityEngine;

namespace Source.Player
{
    public class PlayerHealth : Health
    {
        public static event Action dead;
        public static event Action<float> healthChanged;

        protected override void OnHealthChanged()
        {
            base.OnHealthChanged();
            healthChanged?.Invoke(Value);
        }

        [ContextMenu("Die")]
        protected override void Die()
        {
            base.Die();
            dead?.Invoke();
        }
    }
}