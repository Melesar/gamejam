using System;
using System.Collections.Generic;
using Source.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.PlayerAttack
{
    [Serializable]
    public struct AttackMapping
    {
        public GameState state;
        public AttackController controller;
    }

    public class PlayerAttackHandler : MonoBehaviour
    {
        [SerializeField] private List<AttackMapping> _controllers;
        [SerializeField] private PlayerReferences _references;

        private AttackController _currentController;
        private bool _isDead;

        private void ChangeController(GameState state)
        {
            if (_currentController != null)
            {
                _currentController.Dispose();
            }

            _currentController = _controllers.Find(c => c.state == state).controller;
            _currentController.Init(_references);

            gameObject.ExecuteEvent<IAttackControllerListener>(listener =>
                listener.OnAttackControllerChange(_currentController));
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