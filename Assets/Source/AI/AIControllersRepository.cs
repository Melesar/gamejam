using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.AI
{
    // ReSharper disable once InconsistentNaming
    public class AIControllersRepository
    {
        private readonly GameObject _agent;

        private Transform _transform;
        private readonly Dictionary<Type, Component> _componentsCache;

        public Transform Transform => _transform != null ? _transform : (_transform = _agent.transform);
        
        public T GetComponent<T>() where T : Component
        {
            var type = typeof(T);
            if (_componentsCache.TryGetValue(type, out var component))
            {
                return (T) component;
            }

            var result = _agent.GetComponent<T>();
            if (result == null)
            {
                var ex = new ArgumentException($"Component of type {type} is not found on the agent");
                Debug.LogException(ex, _agent);
                throw ex;
            }
            
            _componentsCache.Add(type, result);
            return result;
        }
        
        public AIControllersRepository(GameObject agent)
        {
            _agent = agent;
            _componentsCache = new Dictionary<Type, Component>();
        }
    }
}