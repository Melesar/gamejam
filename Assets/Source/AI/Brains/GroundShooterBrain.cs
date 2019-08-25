using UnityEngine;

namespace Source.AI
{
    public class GroundShooterBrain : GroundAgentBrain
    {
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
            if (Physics.Raycast(origin, direction, Params.AttackRange, Params.NotPlayerMask))
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

        public GroundShooterBrain(AIContext context,
            AIControllersRepository controllersRepository,
            BrainParams @params) :
            base(context, controllersRepository, @params)
        {
        }
    }
}