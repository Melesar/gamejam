using System;
using UnityEngine;

namespace Source.PlayerAttack
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private GameObject _mesh;
        [SerializeField] private GameObject _particles;
        
        private Rigidbody _rb;
        
        public void Init(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            _rb.velocity = direction * _speed;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return;
            }
            
            _mesh.SetActive(false);
            _particles.SetActive(true);
            Destroy(gameObject, 2f);
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
    }
}