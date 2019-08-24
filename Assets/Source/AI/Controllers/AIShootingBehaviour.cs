using System;
using Source.PlayerAttack;
using UnityEngine;

namespace Source.AI
{
    public class AIShootingBehaviour : MonoBehaviour
    {
        [SerializeField] private PlayerReferences _references;
        [SerializeField] private AIShootingController _controller;
        
        public void Shoot(Vector3 direction)
        {
            _controller.Direction = direction;
            _controller.ShootProjectile();
        }

        private void Update()
        {
            _controller.Update();
        }

        private void Start()
        {
            _controller.Init(_references);
        }
    }
}