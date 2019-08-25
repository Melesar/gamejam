using UnityEngine;

namespace Source.AI
{
    public abstract class GroundAgentBrain : AIBrain
    {
        public class BrainParams
        {
            public float MinTravelDistance { get; set; }
            public float MaxTravelDistance { get; set; }
            public LayerMask NotPlayerMask { get; set; }
            public float AttackRange { get; set; }
            public Gravity Gravity { get; set; }
        }
        
        protected BrainParams Params { get; }
        
        private Vector3 _direction;
        private Vector3 _targetPoint;
        
        protected void Patrol()
        {
            var transform = Transform;
            if (Vector3.Distance(transform.position, _targetPoint) < 0.7f)
            {
                ChooseNewDirection();
            }
            else
            {
                var moveCommand = new MoveCommand {Direction = _direction};
                SubmitCommand(moveCommand);
            }

            Debug.DrawLine(transform.position, _targetPoint, Color.red);
        }
        
        private void ChooseNewDirection()
        {
            var newDirection = Mathf.Sign(Random.Range(-1f, 1f)) * Transform.forward;
            var distance = Random.Range(Params.MinTravelDistance, Params.MaxTravelDistance);

            if (!CheckGround(newDirection, distance))
            {
                return;
            }

            if (Physics.Raycast(Context.raycastOrigin.position, newDirection, out var hit, distance))
            {
                _targetPoint = hit.point;
                _targetPoint.x = Transform.position.x;
            }
            else
            {
                _targetPoint = Transform.position + distance * newDirection;
            }

            _direction = newDirection;
        }

        protected bool CheckGround(Vector3 direction, float distance)
        {
            //Cast to the ground to get the point
            var groundLayer = LayerMask.GetMask("Ground");
            var ray = new Ray(Context.raycastOrigin.position, -Context.raycastOrigin.up);
            if (!Physics.Raycast(ray, out var hit, 10f,
                groundLayer))
            {
                return false;
            }

            var straightDistanceToGround = hit.distance;
            var destinationOnGround = hit.point + direction * distance;
            var raycastDistance = Mathf.Sqrt(straightDistanceToGround * straightDistanceToGround + distance * distance);
            var raycastOrigin = Context.raycastOrigin.position;
            var raycastDirection = Vector3.Normalize(destinationOnGround - raycastOrigin);

            return Physics.Raycast(raycastOrigin, raycastDirection, raycastDistance + 0.1f, groundLayer);
        }
        
        protected GroundAgentBrain(AIContext context, AIControllersRepository controllersRepository, BrainParams @params) : base(context, controllersRepository)
        {
            Params = @params;
            ChooseNewDirection();
            GetComponent<AIGroundMovement>().finishedSwitchingGravity += ChooseNewDirection;
        }
    }
}