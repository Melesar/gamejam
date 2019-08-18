using UnityEngine.EventSystems;

namespace Source.PlayerAttack
{
    public interface IAttackControllerListener : IEventSystemHandler
    {
        void OnAttackControllerChange(PlayerAttackController attackController);
    }
}