using Unity.Collections;
using UnityEngine;

namespace Source.PlayerMovement
{
    public class Raycaster
    {
        private readonly BoxCollider _collider;
        private readonly RaycastSettings _settings;

        private readonly Vector3[] _verticalOrigins;
        private readonly Vector3[] _horizontalOrigins;
        
        private static readonly RaycastHit _noHit = new RaycastHit();

        public bool CastHorizontally(Vector2 velocity, out RaycastHit hit)
        {
            CalculateHorizontalOrigins();

            int raysCount = _settings.horizontalRaysCount;
            var results = new NativeArray<RaycastHit>(raysCount, Allocator.Temp);
            int resultIndex = -1;
            float minDistance = float.MaxValue;
            for (int i = 0; i < raysCount; i++)
            {
                Vector3 origin = _horizontalOrigins[i];
                Vector3 direction = Mathf.Abs(velocity.x) * _collider.transform.forward;
                
                Debug.DrawRay(origin, direction);

                Physics.Raycast(origin, direction, out RaycastHit currentHit, Mathf.Abs(velocity.x) + _settings.skinWidth,
                    _settings.mask);

                results[i] = currentHit;
                if (currentHit.collider != null && currentHit.distance < minDistance)
                {
                    minDistance = currentHit.distance;
                    resultIndex = i;
                }
            }            
            
            hit = resultIndex >= 0 ? results[resultIndex] : _noHit;
            hit.distance = Mathf.Max(hit.distance - _settings.skinWidth, 0f);
            
            results.Dispose();

            return resultIndex >= 0;
        }

        public bool CastVertically(Vector2 velocity, out RaycastHit hit)
        {
            CalculateVerticalOrigins(velocity);

            int raysCount = _settings.verticalRaysCount;
            var results = new NativeArray<RaycastHit>(raysCount, Allocator.Temp);
            int resultIndex = -1;
            float minDistance = float.MaxValue;
            for (int i = 0; i < raysCount; i++)
            {
                Vector3 origin = _verticalOrigins[i];
                Vector3 direction = _collider.transform.up * velocity.y;
                
                Debug.DrawRay(origin, direction);

                Physics.Raycast(origin, direction.normalized, out RaycastHit currentHit, Mathf.Abs(velocity.y) + _settings.skinWidth,
                    _settings.mask);

                results[i] = currentHit;
                if (currentHit.collider != null && currentHit.distance < minDistance)
                {
                    minDistance = currentHit.distance;
                    resultIndex = i;
                }
            }            
            
            hit = resultIndex >= 0 ? results[resultIndex] : _noHit;
            hit.distance = Mathf.Max(hit.distance - _settings.skinWidth, 0f);
            
            results.Dispose();

            return resultIndex >= 0;
        }

        public Raycaster(RaycastSettings settings, BoxCollider collider)
        {
            _settings = settings;
            _collider = collider;
            
            _verticalOrigins = new Vector3[settings.verticalRaysCount];
            _horizontalOrigins = new Vector3[settings.horizontalRaysCount];
        }

        private void CalculateVerticalOrigins(Vector2 velocity)
        {
            Vector3 extents = _collider.size * 0.5f;
            Vector3 offset = new Vector3(0f, Mathf.Sign(velocity.y) * (extents.y - _settings.skinWidth), extents.z);
            Vector3 topCorner = _collider.center + offset;
            
            float raySpacing = _collider.size.z / Mathf.Max(_settings.verticalRaysCount - 1, 1);
            for (int i = 0; i < _settings.verticalRaysCount; i++)
            {
                Vector3 rayOffset = i * raySpacing * -Vector3.forward;
                Vector3 localOrigin = topCorner + rayOffset;

                _verticalOrigins[i] = _collider.transform.TransformPoint(localOrigin);
            }
        }
        
        private void CalculateHorizontalOrigins()
        {
            Vector3 extents = _collider.size * 0.5f;
            Vector3 offset = new Vector3(0f, extents.y, extents.z - _settings.skinWidth);
            Vector3 topCorner = _collider.center + offset;
            
            float raySpacing = _collider.size.y / Mathf.Max(_settings.horizontalRaysCount - 1, 1);
            for (int i = 0; i < _settings.horizontalRaysCount; i++)
            {
                Vector3 rayOffset = i * raySpacing * -Vector3.up;
                Vector3 localOrigin = topCorner + rayOffset;

                _horizontalOrigins[i] = _collider.transform.TransformPoint(localOrigin);
            }
        }
    }
}