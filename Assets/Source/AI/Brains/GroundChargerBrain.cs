using UnityEngine;

namespace Source.AI
{
    public class GroundChargerBrain : AIBrain
    {
        public class BrainParams
        {
            public float MaxTravelDistance { get; set; }
            public LayerMask NotPlayerMask { get; set; }
            public float AttackRange { get; set; }
            public Gravity Gravity { get; set; }
        }
        
        private BrainParams Params { get; }

        private bool IsAttacking => GetComponent<AIGroundChargeAttack>().IsAttacking;
        
        private Vector3 _direction;
        private Vector3 _targetPoint;
        
        public override void UpdateBrain(float dt)
        {
            var movement = GetComponent<AIGroundMovement>();
            if (!movement.IsGrounded)
            {
                return;
            }

            if (IsPlayerReachable())
            {
                if (CanAttackPlayer())
                {
                    AttackPlayer();
                }
                else
                {
                    if (IsAttacking)
                    {
                        SubmitCommand(new StopChargeAttackCommand());
                    }
                    ChasePlayer();
                }
            }
            else
            {
                Patrol();
            }
        }
        
        private bool IsPlayerReachable()
        {
            var playerPosition = Context.player.transform.position;
            var origin = Context.raycastOrigin.position;
            var direction = playerPosition - origin;
            if (Physics.Raycast(origin, direction, direction.magnitude, Params.NotPlayerMask))
            {
                //Player is not visible
                return false;
            }

            var projectedDirection = Vector3.ProjectOnPlane(direction, -Params.Gravity.Value);
            if (Physics.Raycast(origin, projectedDirection, projectedDirection.magnitude, Params.NotPlayerMask))
            {
                return false;
            }
            
            return true;
        }

        private bool CanAttackPlayer()
        {
            var playerPosition = Context.player.transform.position;
            return Vector3.Distance(playerPosition, Transform.position) < Params.AttackRange;
        }

        private void AttackPlayer()
        {
            SubmitCommand(new ChargeCommand {Player = Context.player});
        }

        private void ChasePlayer()
        {
            var playerPosition = Context.player.transform.position;
            var direction = Vector3.ProjectOnPlane(playerPosition - Transform.position, Transform.up);
            direction.Normalize();
            
            SubmitCommand(new MoveCommand {Direction = direction});
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
        
        public GroundChargerBrain(AIContext context,
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