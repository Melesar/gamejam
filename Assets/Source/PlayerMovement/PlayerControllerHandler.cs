using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Player
{
    public class PlayerControllerHandler : MonoBehaviour
    {
        [Serializable]
        public struct ControllerMapping
        {
            public GameState state;
            public PlayerController controller;
        }
        
        [SerializeField] private List<ControllerMapping> _controllers;
        [SerializeField] private PlayerReferences _refs;
        
        private PlayerController _currentController;

        private void ChangeController(GameState state)
        {
            if (_currentController != null)
            {
                _currentController.Dispose();
            }
            
            _currentController = _controllers.Find(c => c.state == state).controller;
            _currentController.Init(_refs);

            gameObject.ExecuteEvent<IControllerListener>(listener => listener.OnControllerChanged(_currentController));
//            ExecuteEvents.Execute<IControllerListener>(gameObject, null,
//                (handler, data) => handler.OnControllerChanged(_currentController));
        }

        private void FixedUpdate()
        {
            _currentController.FixedUpdate();
        }

        private void Update()
        {
            _currentController.Update();
        }

        private void Start()
        {
            ChangeController(GameState.Platformer);
        }
    }
}