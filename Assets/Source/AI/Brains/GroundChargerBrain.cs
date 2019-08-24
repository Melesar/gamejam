using UnityEngine;

namespace Source.AI
{
    [CreateAssetMenu(menuName = "Brains/Ground charger")]
    public class GroundChargerBrainAsset : AIBrainAsset
    {
        [SerializeField] private float _maxTravelDistance = 5f;
        public override AIBrain GetBrain(AIContext context, AIControllersRepository controllersRepository)
        {
            return new GroundChargerBrain(context, controllersRepository, _maxTravelDistance);
        }
    }
    
    public class GroundChargerBrain : AIBrain
    {
        private float MaxTravelDistance { get; }

        private Vector3 _direction;
        private Vector3 _targetPoint;
        
        public override void UpdateBrain(float dt)
        {
            var movement = GetComponent<AIGroundMovement>();
            if (!movement.IsGrounded)
            {
                return;
            }

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
            var distance = Random.Range(0f, MaxTravelDistance);

            if (Physics.Raycast(Context.raycastOrigin.position, newDirection, out var hit, distance))
            {
                _targetPoint = hit.point;
                _targetPoint.z = Transform.position.z;
            }
            else
            {
                _targetPoint = Transform.position + distance * newDirection;
            }

            _direction = newDirection;
        }

        public GroundChargerBrain(
            AIContext context, 
            AIControllersRepository controllersRepository, 
            float maxDistance) :
            base(context, controllersRepository)
        {
            MaxTravelDistance = maxDistance;
            ChooseNewDirection();
        }
    }
}