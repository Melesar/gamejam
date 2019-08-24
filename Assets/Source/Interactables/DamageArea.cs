using System;
using Source.Player;
using UnityEngine;

namespace Source.Interactables
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _interval;

        private float _damageTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                return;
            }

            var health = other.GetComponentInParent<PlayerHealth>();
            health.TakeDamage(_damage);
            _damageTime = _interval;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                return;
            }

            var health = other.GetComponentInParent<PlayerHealth>();
            if (_damageTime <= 0f)
            {
                health.TakeDamage(_damage);
                _damageTime = _interval;
            }
            else
            {
                _damageTime -= Time.deltaTime;
            }
        }
    }
}