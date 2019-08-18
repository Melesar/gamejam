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
        
        public void Init(PlayerReferences refs)
        {
            Refs = refs;
        }
    }
}