using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Source.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Source
{
    [CreateAssetMenu(menuName = "Platformer controller")]
    public class PlatformerController : PlayerController
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _jumpDistance = 3f;

        [Space]
        [SerializeField] private float _obstacleDetectionDistance = 1f;
        
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
        private bool _isFacingObstacle;

        private float DeltaTime => Time.fixedDeltaTime;

        public override void Move(float vertical, float horizontal)
        {
            _velocityZ = vertical * _moveSpeed;

            Rotate(vertical);
            
            var offset = _velocityZ * Time.fixedDeltaTime;
            Translate(new Vector3(0f, 0f, offset));
            
            AnimationProperties.moveDirection = Mathf.Abs(vertical);
        }

        private void Rotate(float vertical)
        {
            var rotation = Refs.rig.localRotation.eulerAngles;
            if (rotation.y.AlmostZero() && vertical < 0f)
            {
                rotation.y = 180f;
                Refs.rig.localRotation = Quaternion.Euler(rotation);
            } 
            else if (Mathf.Approximately(rotation.y, 180f) && vertical > 0f)
            {
                rotation.y = 0f;
                Refs.rig.localRotation = Quaternion.Euler(rotation);
            }
        }

        public override void Jump()
        {
            if (!IsJumping && _isLanded)
            {
                _jumpCoroutine = StartCoroutine(JumpCoroutine());
//                AnimationProperties.jumpDirection = 1f;
            }
        }

        private IEnumerator JumpCoroutine()
        {
            var gravityValue = Gravity.Value.y;
            var velocityY = -gravityValue * 2f * _jumpHeight * _jumpSpeed / _jumpTopDistance;
            var gravity = gravityValue * 2f * _jumpHeight * (_jumpSpeed * _jumpSpeed) /
                          (_jumpTopDistance * _jumpTopDistance);
                
            do
            {
                var dt = DeltaTime;
                var offsetY = velocityY * dt + 0.5f * gravity * dt * dt;
                var offsetZ = _velocityZ * dt;
                var offset = new Vector3(0f, offsetY, offsetZ);

                AnimationProperties.jumpDirection = -Vector3.Dot(offset, Gravity.Value);

                Translate(offset);

                velocityY += gravity * dt;
                
                yield return new WaitForFixedUpdate();
            }
            while (!_isLanded);
            
            _jumpCoroutine = null;
        }

        public override void FixedUpdate()
        {
            AirControl();
            ObstacleDetection();
        }

        private void ObstacleDetection()
        {
            var lookDirection = new Vector3(0f, 0f, _velocityZ);
            lookDirection = Transform.TransformDirection(lookDirection);

            var ray = new Ray(Refs.rigidbody.position, lookDirection);
            _isFacingObstacle =
                Physics.Raycast(ray, _obstacleDetectionDistance, _groundMask);
            
            Debug.DrawLine(ray.origin, ray.GetPoint(_obstacleDetectionDistance), Color.magenta);
        }

        private void AirControl()
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

            AnimationProperties.isGrounded = _isLanded;
            if (_isLanded)
            {
                AnimationProperties.jumpDirection = 0f;
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
            var dot = Vector3.Dot(offset, Gravity.Value);
            if (dot.AlmostZero() && _isFacingObstacle)
            {
                return Vector3.zero;
            }
            
            if (dot <= 0f)
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