using System;
using System.Collections;
using UnityEngine;

namespace Source.Player
{
    public class AnimationProperties
    {
        public float moveDirection;
        public float jumpDirection;
        public bool isGrounded;
    }
    
    public abstract class PlayerController : ScriptableObject
    {
        public AnimationProperties AnimationProperties { get; private set; }
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

        protected void StopCoroutine(Coroutine routine)
        {
            if (routine != null)
            {
                Refs.coroutineHandler.StopCoroutine(routine);
            }
        }

        public virtual void Init(PlayerReferences refs)
        {
            Refs = refs;
            AnimationProperties = new AnimationProperties();
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