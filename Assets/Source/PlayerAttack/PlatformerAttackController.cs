using System.Text;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Source.PlayerAttack
{
    [CreateAssetMenu(menuName = "Platformer attack controller")]
    public class PlatformerAttackController : AttackController
    {
        protected override Vector3 GetShootDirection()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            var mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
            var direction = mousePos - Refs.shootingOrigin.position;
            direction = Vector3.ProjectOnPlane(direction.normalized, Vector3.up);
            
            return direction;
        }
    }
}