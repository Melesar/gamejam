using UnityEngine;

namespace Source.AI
{
    public class HealthController : MonoBehaviour, IHealthListener
    {
        public void OnHealthChanged(float value, float ratio)
        {
        }

        public void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}