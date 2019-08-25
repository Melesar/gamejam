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
            var camera = Camera.main;
            var shootingOrigin = Refs.shootingOrigin.position;
            var direction = shootingOrigin - camera.transform.position;
            direction = Vector3.Project(direction, camera.transform.forward);
            
            var mousePosition = Input.mousePosition;
            mousePosition.z = direction.magnitude;
            var mousePos = camera.ScreenToWorldPoint(mousePosition);
            direction = mousePos - shootingOrigin;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            
            return direction;
        }
    }
}