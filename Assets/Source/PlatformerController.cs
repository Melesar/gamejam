using Source.Player;
using UnityEngine;

namespace Source
{
    [CreateAssetMenu(menuName = "Platformer controller")]
    public class PlatformerController : PlayerController
    {
        [SerializeField] private float _moveSpeed;
        
        [Header("Ground check")]
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private LayerMask _groundMask;

        private bool _isLanded;
        
        public override void Move(float vertical, float horizontal)
        {
            var offset = vertical * _moveSpeed * Time.deltaTime;
            Transform.Translate(new Vector3(0f, 0f, offset));
        }

        public override void FixedUpdate()
        {
            _isLanded = Physics.Raycast(Transform.position, -Gravity.Value, out var hit, _groundCheckDistance, _groundMask);
            if (_isLanded)
            {
                Transform.position = hit.point;
            }
        }

        public override void Update()
        {
            if (!_isLanded)
            {
                Transform.Translate(Gravity.Value, Space.World);
            }
        }

        public override void Jump()
        {
            
        }
    }
}