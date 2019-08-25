using System;
using UnityEngine;

namespace Source.Player
{
    [Serializable]
    public class PlayerReferences
    {
        public Transform transform;
        public Transform rig;
        public Rigidbody rigidbody;
        public MonoBehaviour coroutineHandler;
        public Transform groundContact;
        public Transform obstacleDetectionContact;
        public PlayerCamera camera;
    }
}