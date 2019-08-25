using System;
using Source.PlayerAttack;
using UnityEngine;

namespace Source.AI
{
    public class AIShootingBehaviour : MonoBehaviour
    {
        [SerializeField] private PlayerReferences _references;
        [SerializeField] private AIShootingController _controller;

        private AIShootingController _controllerInstance;
        
        public void Shoot(Vector3 direction)
        {
            _controllerInstance.Direction = direction;
            _controllerInstance.ShootProjectile();
        }

        private void Update()
        {
            _controllerInstance.Update();
        }

        private void Start()
        {
            _controllerInstance = Instantiate(_controller);
            _controllerInstance.Init(_references);
        }
    }
}