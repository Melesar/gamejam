using System;
using UnityEngine;

namespace Source.PlayerMovement
{
    [Serializable]
    public class RaycastSettings
    {
        public int horizontalRaysCount;
        public int verticalRaysCount;
        public float skinWidth;
        public LayerMask mask;
    }
}