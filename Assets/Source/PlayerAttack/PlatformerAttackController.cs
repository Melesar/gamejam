using System.Text;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Source.PlayerAttack
{
    [CreateAssetMenu(menuName = "Platformer attack controller")]
    public class PlatformerAttackController : PlayerAttackController
    {
        protected override Vector3 GetShootDirection()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            var mousePos = Refs.camera.ScreenToWorldPoint(mousePosition);
            var direction = mousePos - Refs.shootingOrigin.position;
            direction = Vector3.ProjectOnPlane(direction, Refs.shootingOrigin.right);
            
            return direction;
        }
    }
}