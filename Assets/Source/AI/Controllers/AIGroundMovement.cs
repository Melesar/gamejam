using System;
using Source.Player;
using UnityEngine;

namespace Source.AI
{
    // ReSharper disable once InconsistentNaming
    public class AIGroundMovement : AIMovement, IChangeGravityListener
    {
        [SerializeField] private bool _useInstanceController;
        [SerializeField] private PlayerReferences _references;
        [SerializeField] private PlatformerController _controller;
        [SerializeField] private Animator _animator;

        private PlatformerController _controllerInstance;
        private readonly int _motionAnimationHash = Animator.StringToHash("motion");
        
        public event Action finishedSwitchingGravity;
        
        public bool IsGrounded => _controllerInstance.IsGrounded;

        public override void Move(Vector3 direction)
        {
            direction = Vector3.Project(direction, transform.forward);
            _controllerInstance.Move(direction.z, 0f);
            
            _animator.SetFloat(_motionAnimationHash, Mathf.Abs(direction.z));
        }

        private void FixedUpdate()
        {
            _controllerInstance.FixedUpdate();
        }

        private void Start()
        {
            _controllerInstance = _useInstanceController ? Instantiate(_controller) : _controller;
            _controllerInstance.Init(_references);
            _controllerInstance.AffectedByGravity = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("GravityVolume"))
            {
                _controllerInstance.AffectedByGravity = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("GravityVolume"))
            {
                _controllerInstance.AffectedByGravity = false;
            }
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