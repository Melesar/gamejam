using System.Collections;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Source.AI
{
    // ReSharper disable once InconsistentNaming
    public class AIGroundChargeAttack : MonoBehaviour
    {
        [SerializeField] private float _damage = 15f;
        [SerializeField] private float _chargeSpeed = 10f;
        [SerializeField] private float _attacksInterval = 1.5f;
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;
        
        public bool IsAttacking => _attackCoroutine != null;

        private readonly int _isAttackAnimationHash = Animator.StringToHash("isAttacking");
        private readonly int _attackStateAnimationHash = Animator.StringToHash("attackState");

        private Coroutine _attackCoroutine;
        private Sequence _sequence;

        public void StartAttacking(GameObject player)
        {
            if (!IsAttacking)
            {
                _animator.SetBool(_isAttackAnimationHash, true);
                _attackCoroutine = StartCoroutine(AttackCoroutine(player));
            }
        }

        public void StopAttacking()
        {
            _sequence?.Kill();
            _collider.enabled = true;
            _animator.SetBool(_isAttackAnimationHash, false);
            
            if (IsAttacking)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }

        private IEnumerator AttackCoroutine(GameObject player)
        {
            while (true)
            {
                var currentPosition = transform.position;
                var targetPosition = player.transform.position;
                var totalDistance = Vector3.Distance(currentPosition, targetPosition);
                var t = totalDistance / _chargeSpeed;
                
                _sequence = DOTween.Sequence();
                _sequence.AppendCallback(() => _collider.enabled = false);
                _sequence.Append(transform.DOMove(targetPosition, t).SetEase(Ease.Linear));
                _sequence.AppendCallback(() => TryDoDamage(player));
                _sequence.Append(transform.DOMove(currentPosition, t).SetEase(Ease.Linear));
                _sequence.AppendCallback(() => _collider.enabled = true);
                _sequence.Play();

                while (!_sequence.IsComplete())
                {
                    var currentDistance = Vector3.Distance(transform.position, currentPosition);
                    _animator.SetFloat(_attackStateAnimationHash, currentDistance / totalDistance);
                    yield return null;
                }

                yield return new WaitForSeconds(_attacksInterval);
            }
        }

        private void TryDoDamage(GameObject player)
        {
            player.GetComponent<Health>()?.TakeDamage(_damage);
        }
    }
}