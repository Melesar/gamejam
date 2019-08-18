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

        private void ChangeController(GameState state)
        {
            var controller = _controllers.Find(c => c.state == state).controller;
            controller.Init(_refs);

            ExecuteEvents.Execute<IControllerListener>(gameObject, null,
                (handler, data) => handler.OnControllerChanged(controller));
        }

        private void Start()
        {
            ChangeController(GameState.Platformer);
        }
    }
}