using UnityEngine;

namespace Source.AI
{
    [CreateAssetMenu(menuName = "Brains/Ground charger")]
    public class GroundChargerBrainAsset : AIBrainAsset
    {
        [SerializeField] private float _minTravelDistance = 2f;
        [SerializeField] private float _maxTravelDistance = 5f;
        [SerializeField] private float _attackRange = 7f;
        [SerializeField] private LayerMask _notPlayerMask;
        [SerializeField] private Gravity _gravity;
        
        public override AIBrain GetBrain(AIContext context, AIControllersRepository controllersRepository)
        {
            return new GroundChargerBrain(context, controllersRepository, new GroundAgentBrain.BrainParams
            {
                NotPlayerMask = _notPlayerMask,
                MinTravelDistance = _maxTravelDistance,
                MaxTravelDistance = _maxTravelDistance,
                AttackRange = _attackRange,
                Gravity = _gravity
            });
        }
    }
}