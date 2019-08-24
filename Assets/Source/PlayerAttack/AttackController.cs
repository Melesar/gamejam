using UnityEngine;

namespace Source.PlayerAttack
{
    public abstract class AttackController : ScriptableObject
    {
        [SerializeField] protected float _shootCooldown;
        [SerializeField] protected Projectile _projectilePrefab;

        private float _currentShootCooldown;
        
        protected PlayerReferences Refs { get; private set; }
        
        public void Init(PlayerReferences refs)
        {
            Refs = refs;
        }

        public void ShootProjectile()
        {
            if (_currentShootCooldown > 0f)
            {
                return;
            }

            var direction = GetShootDirection();
            var projectile = Instantiate(_projectilePrefab, Refs.shootingOrigin, false);
            projectile.Init(direction);

            _currentShootCooldown = _shootCooldown;
        }

        protected abstract Vector3 GetShootDirection();

        public virtual void Update()
        {
            _currentShootCooldown -= Time.deltaTime;
        }

        public virtual void Dispose()
        {
            
        }
    }
}