using UnityEngine;

namespace Source.AI
{
    public class GroundShooterBrain : AIBrain
    {
        public class BrainParams
        {
            public float MaxTravelDistance;
            public LayerMask NoPlayerMask { get; set; }
            public float AttackRange { get; set; }
        }

        private Vector3 _targetPoint;
        private Vector3 _direction;

        private BrainParams Params { get; }
        
        public override void UpdateBrain(float dt)
        {
            var movement = GetComponent<AIGroundMovement>();
            if (!movement.IsGrounded)
            {
                return;
            }

            if (CanAttackPlayer())
            {
                AttackPlayer();
            }
            else
            {
                Patrol();
            }
        }

        private bool CanAttackPlayer()
        {
            var playerPosition = Context.player.transform.position;
            var origin = Context.raycastOrigin.position;
            var direction = Vector3.Normalize(playerPosition - Transform.position);
            if (Physics.Raycast(origin, direction, Params.AttackRange, Params.NoPlayerMask))
            {
                //Player is not visible
                return false;
            }

            return Vector3.Distance(Transform.position, playerPosition) <= Params.AttackRange;
        }

        private void AttackPlayer()
        {
            var playerPosition = Context.player.transform.position;
            var direction = Vector3.Normalize(playerPosition - Transform.position);
            
            SubmitCommand(new ShootCommand{Direction = direction});
        }

        private void Patrol()
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
            var distance = Random.Range(0f, Params.MaxTravelDistance);

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
        
        public GroundShooterBrain(AIContext context,
            AIControllersRepository controllersRepository,
            BrainParams @params) :
            base(context, controllersRepository)
        {
            Params = @params;
            ChooseNewDirection();
            GetComponent<AIGroundMovement>().finishedSwitchingGravity += ChooseNewDirection;
        }
    }
}