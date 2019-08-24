using UnityEngine;

namespace Source.AI
{
    // ReSharper disable once InconsistentNaming
    public abstract class AIMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        
        public virtual void Move(Vector3 direction)
        {
            transform.Translate(Time.deltaTime * _moveSpeed * direction);    
        }
    }
}