using System;
using Source.Player;
using Source.PlayerAttack;
using UnityEngine;

namespace Source
{
    public class PlayerInput : MonoBehaviour, IControllerListener, IAttackControllerListener
    {
        [SerializeField] private float _doubleJumpTime = 0.5f;
        
        private PlayerController _motionController;
        private PlayerAttackController _attackController;

        private bool _isJumpPressed;
        private float _doubleJumpTimer;
        
        private void Update()
        {
            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            _motionController.Move(vertical, horizontal);

            if (Input.GetButton("Fire1"))
            {
                _attackController.ShootProjectile();
            }

            if (IsJump())
            {
                _motionController.Jump();
            }

            if (DetectDoubleJump())
            {
                Gravity.Switch();
            }
        }

        private static bool IsJump()
        {
            return Input.GetButtonDown("Jump");
        }

        private bool DetectDoubleJump()
        {
            _doubleJumpTimer -= Time.deltaTime;

            if (_doubleJumpTimer < 0f && _isJumpPressed)
            {
                _isJumpPressed = false;
            }
            
            if (!IsJump())
            {
                return false;
            }

            if (!_isJumpPressed)
            {
                _isJumpPressed = true;
                _doubleJumpTimer = _doubleJumpTime;
                return false;
            }
            
            if (_doubleJumpTimer > 0f)
            {
                _isJumpPressed = false;
                return true;
            }

            return false;
        }

        public void OnControllerChanged(PlayerController controller)
        {
            _motionController = controller;
        }

        public void OnAttackControllerChange(PlayerAttackController attackController)
        {
            _attackController = attackController;
        }
    }
}