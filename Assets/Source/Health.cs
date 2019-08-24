using UnityEngine;

namespace Source
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        
        private float _health;

        public float Value
        {
            get => _health;
            protected set
            {
                var oldValue = Value;
                _health = value;
                if (!Mathf.Approximately(oldValue, value))
                {
                    OnHealthChanged();
                }
            }
        }

        public void TakeDamage(float damage)
        {
            Value = Mathf.Max(0f, Value - damage);
            if (Value.AlmostZero())
            {
                Die();
            }
        }

        protected virtual void OnHealthChanged()
        {
            gameObject.ExecuteEvent<IHealthListener>(listener => listener.OnHealthChanged(Value, Value / _maxHealth));
        }
        
        [ContextMenu("Die")]
        protected virtual void Die()
        {
            gameObject.ExecuteEvent<IHealthListener>(listener => listener.OnDeath());
        }

        private void Start()
        {
            Value = _maxHealth;
        }
    }
}