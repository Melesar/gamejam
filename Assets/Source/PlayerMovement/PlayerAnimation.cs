using System;
using UnityEngine;

namespace Source.Player
{
    public class PlayerAnimation : MonoBehaviour, IControllerListener
    {
        [SerializeField] private Animator _animator;

        private static readonly int _moveDirectionProp = Animator.StringToHash("MovementDirection");
        private static readonly int _jumpDirectionProp = Animator.StringToHash("JumpDirection");
        private static readonly int _isGroundedProp = Animator.StringToHash("IsGrounded");
        
        private PlayerController _controller;
        
        private void Update()
        {
            var props = _controller.AnimationProperties;
            _animator.SetFloat(_moveDirectionProp, props.moveDirection);
            _animator.SetFloat(_jumpDirectionProp, props.jumpDirection);
            _animator.SetBool(_isGroundedProp, props.isGrounded);
        }

        public void OnControllerChanged(PlayerController controller)
        {
            _controller = controller;
        }
    }
}