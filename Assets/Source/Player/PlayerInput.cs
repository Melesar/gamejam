using System;
using Source.Player;
using UnityEngine;

namespace Source
{
    public class PlayerInput : MonoBehaviour, IControllerListener
    {
        [SerializeField] private float _doubleJumpTime = 0.5f;
        private PlayerController _controller;

        private bool _isJumpPressed;
        private float _doubleJumpTimer;
        
        private void Update()
        {
            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            _controller.Move(vertical, horizontal);

            if (IsJump())
            {
                _controller.Jump();
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
            _controller = controller;
        }
    }
}