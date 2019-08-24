using UnityEngine;

namespace Source.AI
{
    [CreateAssetMenu(menuName = "Brains/Ground shooter")]
    public class GroundShooterBrainAsset : AIBrainAsset
    {
        [SerializeField] private float _maxTravelDistance;
        [SerializeField] private LayerMask _noPlayerMask;
        [SerializeField] private float _attackRange;
        
        public override AIBrain GetBrain(AIContext context, AIControllersRepository controllersRepository)
        {
            return new GroundShooterBrain(context, controllersRepository, new GroundShooterBrain.BrainParams
            {
                MaxTravelDistance = _maxTravelDistance,
                NoPlayerMask = _noPlayerMask,
                AttackRange = _attackRange,
            });
        }
    }
}