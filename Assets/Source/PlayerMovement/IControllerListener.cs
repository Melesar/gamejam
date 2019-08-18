using UnityEngine.EventSystems;

namespace Source.Player
{
    public interface IControllerListener : IEventSystemHandler
    {
        void OnControllerChanged(PlayerController controller);
    }
}