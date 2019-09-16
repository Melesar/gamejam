using System;
using UnityEngine;

namespace Source.AI
{
    public class HealthController : MonoBehaviour, IHealthListener
    {
        [SerializeField] private Animator _animator;

        private readonly int _deathAnimationHash = Animator.StringToHash("death");
        
        public void OnHealthChanged(float value, float ratio)
        {
        }

        public void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}