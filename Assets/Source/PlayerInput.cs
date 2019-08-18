using System;
using Source.Player;
using UnityEngine;

namespace Source
{
    public class PlayerInput : MonoBehaviour, IControllerListener
    {
        private PlayerController _controller;
        
        private void Update()
        {
            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            _controller.Move(vertical, horizontal);

            if (Input.GetButtonDown("Jump"))
            {
                _controller.Jump();
            }
        }

        public void OnControllerChanged(PlayerController controller)
        {
            _controller = controller;
        }
    }
}