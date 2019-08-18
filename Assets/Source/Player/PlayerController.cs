using UnityEngine;

namespace Source.Player
{
    public abstract class PlayerController : ScriptableObject
    {
        public abstract void Move(float vertical, float horizontal);

        public abstract void Jump();
        
        public void Init(PlayerReferences refs)
        {
            
        }
    }
}