using System;
using UnityEngine;

namespace Source
{
    [RequireComponent(typeof(PlatformerController))]
    public class PlayerInput : MonoBehaviour
    {
        private PlatformerController _controller;
        
        private void Update()
        {
            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            _controller.Move(vertical, horizontal);
        }

        private void Awake()
        {
            _controller = GetComponent<PlatformerController>();
        }
    }
}