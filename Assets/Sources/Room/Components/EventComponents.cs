#pragma warning disable 649
using UnityEngine;
using System;

namespace Sources
{
    public struct PushEvent
    {
        public Vector3 position;
        public Vector3 direction;
        public float power;
        public int tick;
    }
    public struct DieEvent { }
    public struct HoleComponent { }

    public struct StayEvent
    {
        public Vector3 position;
    }
}