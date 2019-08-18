using System;
using UnityEngine;

namespace Source.Player
{
    [Serializable]
    public class PlayerReferences
    {
        public Transform transform;
        public Rigidbody rigidbody;
        public MonoBehaviour coroutineHandler;
    }
}