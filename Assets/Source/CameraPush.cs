using Source.Player;
using UnityEngine;

namespace Source
{
    [RequireComponent(typeof(Camera))]
    public class CameraPush : MonoBehaviour
    {
        [SerializeField, Range(0, 0.5f)] private float _screenOffset;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private PlatformerController _playerController;
        
        private Camera _camera;

        private Camera Camera => _camera != null ? _camera : (_camera = GetComponent<Camera>());

        private void Update()
        {
            var velocity = _playerController.Velocity;
            var playerViewportPos = Camera.WorldToViewportPoint(_playerTransform.position);
            if (playerViewportPos.y < _screenOffset && velocity <= 0f)
            {
                playerViewportPos.y = _screenOffset;
                _playerTransform.position = Camera.ViewportToWorldPoint(playerViewportPos);
            }
            else if (playerViewportPos.y > 1f - _screenOffset && velocity >= 0f)
            {
                playerViewportPos.y = 1f - _screenOffset;
                _playerTransform.position = Camera.ViewportToWorldPoint(playerViewportPos);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            void DrawLine(float y)
            {
                var left = Camera.ViewportToWorldPoint(new Vector3(0f, y, 10));
                var right = Camera.ViewportToWorldPoint(new Vector3(1f, y, 10));

                Gizmos.DrawLine(left, right);
            }

            DrawLine(1f - _screenOffset);
            DrawLine(_screenOffset);
        }
    }
}