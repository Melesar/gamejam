using System;
using UnityEngine;

namespace Source.Interactables
{
    public class MovingPlatform : MonoBehaviour
    {
        public enum PlatformBehaviour
        {
            Loop, PingPong
        }

        [SerializeField] private float _moveSpeed;
        [SerializeField] private Transform _platform;
        [SerializeField] private Transform[] _points;
        [SerializeField] private PlatformBehaviour _behaviour;
        
        private Vector3 _targetPosition;
        private int _currentPlatformIndex;

        private void Update()
        {
            if (Vector3.Distance(_platform.position, _targetPosition) < 0.5f)
            {
                UpdateTarget();
            }
            else
            {
                var offset = _moveSpeed * Time.deltaTime * (_targetPosition - _platform.position).normalized;
                _platform.Translate(offset, Space.World);
            }
        }

        private int _pingPongDirection = 1;
        
        private void UpdateTarget()
        {
            switch (_behaviour)
            {
                case PlatformBehaviour.Loop:
                    _currentPlatformIndex = (_currentPlatformIndex + 1) % _points.Length;
                    break;
                case PlatformBehaviour.PingPong:
                    if (_currentPlatformIndex + _pingPongDirection < 0)
                    {
                        _pingPongDirection = 1;
                        _currentPlatformIndex = Mathf.Max(0, 1);
                    }
                    else if (_currentPlatformIndex + _pingPongDirection == _points.Length)
                    {
                        _pingPongDirection = -1;
                        _currentPlatformIndex = Mathf.Max(0, _points.Length - 2);
                    }
                    else
                    {
                        _currentPlatformIndex += _pingPongDirection;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _targetPosition = _points[_currentPlatformIndex].position;
        }

        private void Start()
        {
            _currentPlatformIndex = 0;
            _targetPosition = _points[_currentPlatformIndex].position;
        }
    }
}