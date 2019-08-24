using System;
using UnityEngine;

namespace Source.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        
        private float _health;

        public static event Action dead;
        public static event Action<float> healthChanged;

        public float Health
        {
            get => _health;
            private set
            {
                var isHealthChanged = !Mathf.Approximately(value, _health);
                _health = value;
                if (isHealthChanged)
                {
                    Debug.Log($"Health changed to {_health}");
                    healthChanged?.Invoke(_health);
                }
            }
            
        }

        public void TakeDamage(float damage)
        {
            Health = Mathf.Max(0f, Health - damage);
            if (Health.AlmostZero())
            {
                Die();
            }
        }
        
        [ContextMenu("Die")]
        private void Die()
        {
            dead?.Invoke();
        }

        private void Start()
        {
            Health = _maxHealth;
        }
    }
}