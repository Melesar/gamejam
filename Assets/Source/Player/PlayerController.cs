using System;
using System.Collections;
using UnityEngine;

namespace Source.Player
{
    public abstract class PlayerController : ScriptableObject
    {
        protected PlayerReferences Refs { get; private set; }

        protected Transform Transform => Refs.transform;
        
        public abstract void Move(float vertical, float horizontal);

        public abstract void Jump();

        public virtual void FixedUpdate()
        {
            
        }

        public virtual void Update()
        {
            
        }

        protected Coroutine StartCoroutine(IEnumerator routine)
        {
            return Refs.coroutineHandler.StartCoroutine(routine);
        }

        public virtual void Init(PlayerReferences refs)
        {
            Refs = refs;
        }

        public virtual void Dispose()
        {
            
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}