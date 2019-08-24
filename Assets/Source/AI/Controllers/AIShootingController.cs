using Source.PlayerAttack;
using UnityEngine;

namespace Source.AI
{
    [CreateAssetMenu(menuName = "AI shooting controller")]
    public class AIShootingController : AttackController
    {
        public Vector3 Direction { get; set; }
        
        protected override Vector3 GetShootDirection()
        {
            return Direction;
        }
    }
}