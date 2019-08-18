using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.PlayerAttack
{
    [Serializable]
    public struct AttackMapping
    {
        public GameState state;
        public PlayerAttackController controller;
    }
    
    public class PlayerAttackHandler : MonoBehaviour
    {
        [SerializeField] private List<AttackMapping> _controllers;
        [SerializeField] private PlayerReferences _references;
        
        private PlayerAttackController _currentController;

        private void ChangeController(GameState state)
        {
            if (_currentController != null)
            {
                _currentController.Dispose();
            }
            
            _currentController = _controllers.Find(c => c.state == state).controller;
            _currentController.Init(_references);

            ExecuteEvents.Execute<IAttackControllerListener>(gameObject, null,
                (handler, data) => handler.OnAttackControllerChange(_currentController));
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