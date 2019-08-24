using System;
using Source.Player;
using Source.PlayerAttack;
using UnityEngine;

namespace Source
{
    public class PlayerInput : MonoBehaviour, IControllerListener, IAttackControllerListener, IChangeGravityListener
    {
        [SerializeField] private Gravity _gravity;
        [SerializeField] private float _doubleJumpTime = 0.5f;
        
        private PlayerController _motionController;
        private AttackController _attackController;

        private bool _isDead;
        private bool _isChangingGravity;
        private bool _isJumpPressed;
        private float _doubleJumpTimer;
        
        private void Update()
        {
            if (_isDead)
            {
                return;
            }
            
            if (Input.GetButton("Fire1"))
            {
                _attackController.ShootProjectile();
            }

            if (_isChangingGravity)
            {
                return;
            }

            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            _motionController.Move(vertical, horizontal);

            if (DetectDoubleJump())
            {
                _gravity.Switch();
            }
            else if (IsJump())
            {
                _motionController.Jump();
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

        public void OnAttackControllerChange(AttackController attackController)
        {
            _attackController = attackController;
        }

        public void OnGravityChangeStarted()
        {
            _isChangingGravity = true;
        }

        public void OnGravityChangeFinished()
        {
            _isChangingGravity = false;
        }

        private void Start()
        {
            PlayerHealth.dead += OnDeath;
        }

        private void OnDeath()
        {
            _isDead = true;
        }

        private void OnDestroy()
        {
            PlayerHealth.dead -= OnDeath;
        }
    }
}