using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Source.Player;
using UnityEngine;

namespace Source
{
    [CreateAssetMenu(menuName = "Platformer controller")]
    public class PlatformerController : PlayerController
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _jumpDistance = 3f;
        
        [Header("Ground check")]
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private float _groundSafetyDistance;
        [SerializeField] private LayerMask _groundMask;

        private bool IsJumping => _jumpCoroutine != null;
        
        private Coroutine _jumpCoroutine;
        private Vector3 _worldGravity;
        private bool _isLanded;
        private float _currentSafetyDistance;
        private float _jumpTopDistance;
        private float _velocityZ;

        private float DeltaTime => Time.fixedDeltaTime;

        public override void Move(float vertical, float horizontal)
        {
            _velocityZ = vertical * _moveSpeed;
            
            var offset = _velocityZ * Time.deltaTime;
            Translate(new Vector3(0f, 0f, offset));
        }

        public override void Jump()
        {
            if (!IsJumping && _isLanded)
            {
                _jumpCoroutine = StartCoroutine(JumpCoroutine());
            }
        }

        private IEnumerator JumpCoroutine()
        {
            var gravityValue = Gravity.Value.y;
            var velocityY = -gravityValue * 2f * _jumpHeight * _moveSpeed / _jumpTopDistance;
            var gravity = gravityValue * 2f * _jumpHeight * (_moveSpeed * _moveSpeed) /
                          (_jumpTopDistance * _jumpTopDistance);
                
            do
            {
                var dt = DeltaTime;
                var offsetY = velocityY * dt + 0.5f * gravity * dt * dt;
                var offsetZ = _velocityZ * dt;
                var offset = new Vector3(0f, offsetY, offsetZ);

                Translate(offset);

                velocityY += gravity * dt;
                
                yield return new WaitForFixedUpdate();
            }
            while (!_isLanded);
            
            _jumpCoroutine = null;
        }

        public override void FixedUpdate()
        {
            var ray = new Ray(Refs.rigidbody.position, _worldGravity);
            _isLanded = Physics.Raycast(ray, out var hit, _groundCheckDistance, _groundMask);
            _currentSafetyDistance = Physics.Raycast(ray, out var safetyHit, _groundSafetyDistance, _groundMask) 
                ? safetyHit.distance
                : _groundSafetyDistance;

            Debug.DrawLine(ray.origin, ray.GetPoint(_groundCheckDistance));
            Debug.DrawLine(ray.origin, ray.GetPoint(_groundSafetyDistance), Color.green);
            
            if (!IsJumping && !_isLanded)
            {
                Translate(_moveSpeed * DeltaTime * Gravity.Value);
            }
        }

        public override void Init(PlayerReferences refs)
        {
            base.Init(refs);
            
            _jumpTopDistance = _jumpDistance / 2f;

            Gravity.onGravitySwitched += OnGravityChanged;
            
            TransformGravity();
        }

        private void OnGravityChanged()
        {
            TransformGravity();
            
            StopCoroutine(_jumpCoroutine);
            _jumpCoroutine = null;
            
//            Refs.collider.enabled = false;
            StartCoroutine(RotationCoroutine());
        }

        private IEnumerator RotationCoroutine()
        {
            var currentRotation = Refs.rig.rotation;
            var targetEulerAngles = currentRotation.eulerAngles;
            targetEulerAngles.z = -targetEulerAngles.z;
            var targetRotation = Quaternion.Euler(targetEulerAngles);

            const float rotationTime = 1f;
            var currentTime = 0f;
            while (currentTime < rotationTime)
            {
                var t = currentTime / rotationTime;
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                Refs.rig.rotation = Quaternion.Slerp(currentRotation, targetRotation, t);

                currentTime += Time.deltaTime;
                
                yield return null;
            }
        }

        public override void Dispose()
        {
            Gravity.onGravitySwitched -= OnGravityChanged;
        }

        private void TransformGravity()
        {
            _worldGravity = Transform.TransformDirection(Gravity.Value);
        }

        private void Translate(float x, float y, float z)
        {
            Translate(new Vector3(x, y, z));
        }

        private Vector3 ApplySafety(Vector3 offset)
        {
            if (Vector3.Dot(offset, Gravity.Value) <= 0f)
            {
                return offset;
            }
            
            if (Mathf.Approximately(_currentSafetyDistance, _groundSafetyDistance))
            {
                return offset;
            }

            var maxMagnitude = _currentSafetyDistance - _groundCheckDistance + 0.02f;
            if (offset.magnitude < maxMagnitude)
            {
                return offset;
            }

            return offset.normalized * maxMagnitude;
        }

        private void Translate(Vector3 translation)
        {
            translation = ApplySafety(translation);
            var offset = Transform.TransformVector(translation);
            Refs.rigidbody.position += offset;
        }
    }
}