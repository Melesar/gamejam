using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source
{
    public static class Extensions
    {
        public static bool AlmostZero(this float v)
        {
            return Mathf.Approximately(v, 0f);
        }

        public static void ExecuteEvent<T>(this GameObject obj, Action<T> action) where T : IEventSystemHandler
        {
            ExecuteEvents.Execute<T>(obj, null, (handler, data) => action.Invoke(handler));
        }
    }
}