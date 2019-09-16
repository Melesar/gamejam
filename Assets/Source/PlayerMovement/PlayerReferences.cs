using System;
using UnityEngine;

namespace Source.Player
{
    [Serializable]
    public class PlayerReferences
    {
        public Transform transform;
        public Transform rig;
        public BoxCollider raycastCollider;
        public MonoBehaviour coroutineHandler;
        public PlayerCamera camera;
    }
}