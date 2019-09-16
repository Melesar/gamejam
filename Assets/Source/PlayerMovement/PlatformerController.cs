using Source.Player;
using Source.PlayerMovement;
using UnityEngine;

namespace Source
{
    [CreateAssetMenu(menuName = "Platformer controller")]
    public class PlatformerController : PlayerController
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpTime;
        [SerializeField] private float _rotationTime = 0.5f;

        [SerializeField] private RaycastSettings _raycastSettings;
        
        [Space]

        [SerializeField] private Gravity _gravity;

        public bool IsGrounded { get; private set; }
        public bool AffectedByGravity { get; set; } = true;
        
        private float DeltaTime => Time.fixedDeltaTime;

        private float _jumpVelocity;
        private float _jumpGravity;
        private Vector2 _velocity;
        private Raycaster _raycaster;
        
        public override void Move(float vertical, float horizontal)
        {
            _velocity.x = vertical * _moveSpeed;
        }

        public override void Jump()
        {
            _velocity.y = _jumpVelocity;
        }

        public override void FixedUpdate()
        {
            _velocity.y += Mathf.Sign(_gravity.Value) * _jumpGravity * DeltaTime;

            Vector2 velocity = _velocity * DeltaTime; 
            Move(ref velocity);
            Transform.localPosition += new Vector3(0f, velocity.y, velocity.x);
        }

        public override void OnCollision(Collision other)
        {
        }

        public override void OnCollisionExit(Collision other)
        {
        }

        public override void Init(PlayerReferences refs)
        {
            base.Init(refs);
            CalculateJumpStats();

            _velocity.Set(0f, 0f);
            _raycaster = new Raycaster(_raycastSettings, Refs.raycastCollider);
        }

        public override void Dispose()
        {
        }

        private void Move(ref Vector2 velocity)
        {
            if (_raycaster.CastVertically(velocity, out RaycastHit hit))
            {
                velocity.y = Mathf.Sign(velocity.y) * hit.distance;
                _velocity.y = 0;
            }
        }

        private void CalculateJumpStats()
        {
            _jumpGravity = 2f * _jumpHeight / (_jumpTime * _jumpTime);
            _jumpVelocity = _jumpGravity * _jumpTime;
        }
    }
}