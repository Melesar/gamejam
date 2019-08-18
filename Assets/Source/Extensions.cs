using UnityEngine;

namespace Source
{
    public static class Extensions
    {
        public static bool AlmostZero(this float v)
        {
            return Mathf.Approximately(v, 0f);
        }
    }
}