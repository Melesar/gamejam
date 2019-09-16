using UnityEngine;

namespace Source.AI
{
    public class GroundChargerBrain : GroundAgentBrain
    {
        private bool IsAttacking => GetComponent<AIGroundChargeAttack>().IsAttacking;

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

//            var projectedDirection = Vector3.ProjectOnPlane(direction, -Params.Gravity.Value);
//            if (Physics.Raycast(origin, projectedDirection, projectedDirection.magnitude, Params.NotPlayerMask))
//            {
//                return false;
//            }
            
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
            var chaseDistance = direction.magnitude;

            if (!CheckGround(direction, chaseDistance))
            {
                return;
            }
            
            direction.Normalize();
            
            SubmitCommand(new MoveCommand {Direction = direction});
        }

        public GroundChargerBrain(AIContext context,
            AIControllersRepository controllersRepository,
            BrainParams @params) :
            base(context, controllersRepository, @params)
        {
        }
    }
}