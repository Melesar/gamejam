using System;
using Source.Player;
using UnityEngine;

namespace Source.AI
{
    // ReSharper disable once InconsistentNaming
    public class AIGroundMovement : AIMovement, IChangeGravityListener
    {
        [SerializeField] private PlayerReferences _references;
        [SerializeField] private PlatformerController _controller;

        private PlatformerController _controllerInstance;
        
        public event Action finishedSwitchingGravity;
        
        public bool IsGrounded => _controllerInstance.IsGrounded;

        public override void Move(Vector3 direction)
        {
            direction = Vector3.Project(direction, transform.forward);
            _controllerInstance.Move(direction.z, 0f);
        }

        private void FixedUpdate()
        {
            _controllerInstance.FixedUpdate();
        }

        private void Start()
        {
            _controllerInstance = Instantiate(_controller);
            _controllerInstance.Init(_references);
        }

        public void OnGravityChangeFinished()
        {
            finishedSwitchingGravity?.Invoke();
        }

        public void OnGravityChangeStarted()
        {
        }

        private void OnDestroy()
        {
            _controllerInstance.Dispose();
        }
    }
}