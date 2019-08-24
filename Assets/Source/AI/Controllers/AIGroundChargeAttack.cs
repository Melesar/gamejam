using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Source.AI
{
    // ReSharper disable once InconsistentNaming
    public class AIGroundChargeAttack : MonoBehaviour
    {
        [SerializeField] private float _damage = 15f;
        [SerializeField] private float _chargeSpeed = 10f;
        [SerializeField] private float _attacksInterval = 1.5f;
        [SerializeField] private Collider _collider;
        
        public bool IsAttacking => _attackCoroutine != null;

        private Coroutine _attackCoroutine;
        private Sequence _sequence;

        public void StartAttacking(GameObject player)
        {
            if (!IsAttacking)
            {
                _attackCoroutine = StartCoroutine(AttackCoroutine(player));
            }
        }

        public void StopAttacking()
        {
            _sequence?.Kill();
            _collider.enabled = true;
            
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
                var t = Vector3.Distance(currentPosition, targetPosition) / _chargeSpeed;
                
                _sequence = DOTween.Sequence();
                _sequence.AppendCallback(() => _collider.enabled = false);
                _sequence.Append(transform.DOMove(targetPosition, t).SetEase(Ease.Linear));
                _sequence.AppendCallback(() => TryDoDamage(player));
                _sequence.Append(transform.DOMove(currentPosition, t).SetEase(Ease.Linear));
                _sequence.AppendCallback(() => _collider.enabled = true);
                _sequence.Play();

                yield return _sequence.WaitForCompletion();

                yield return new WaitForSeconds(_attacksInterval);
            }
        }

        private void TryDoDamage(GameObject player)
        {
            player.GetComponent<Health>()?.TakeDamage(_damage);
        }
    }
}