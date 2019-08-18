using UnityEngine.EventSystems;

namespace Source
{
    public interface IChangeGravityListener : IEventSystemHandler
    {
        void OnGravityChangeStarted();
        void OnGravityChangeFinished();
    }
}