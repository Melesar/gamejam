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

        public event Action finishedSwitchingGravity;
        
        public bool IsGrounded => _controller.IsGrounded;

        public override void Move(Vector3 direction)
        {
            direction = Vector3.Project(direction, transform.forward);
            _controller.Move(direction.z, 0f);
        }

        private void FixedUpdate()
        {
            _controller.FixedUpdate();
        }

        private void Start()
        {
            _controller.Init(_references);
        }

        public void OnGravityChangeFinished()
        {
            finishedSwitchingGravity?.Invoke();
        }

        public void OnGravityChangeStarted()
        {
        }
    }
}