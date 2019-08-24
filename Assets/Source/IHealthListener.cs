using UnityEngine.EventSystems;

namespace Source
{
    public interface IHealthListener : IEventSystemHandler
    {
        void OnHealthChanged(float value, float ratio);
        void OnDeath();
    }
}