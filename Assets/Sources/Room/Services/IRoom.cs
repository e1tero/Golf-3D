#pragma warning disable 649
using UnityEngine;
using System;

namespace Sources
{
    public interface IRoom
    {
        int Id { get; }
        int Player { get; }
        int Tick { get; }
        bool Connected { get; }
        Ball[] Balls { get; }
        
        void Join(int id);
        void Leave();

        void RegisterStay(Vector3 position);
        void RegisterPush(Vector3 position, Vector3 direction, float power);
    }

    [Serializable]
    public struct Ball
    {
        public int player;
        public BallEvent[] events;
    }

    [Serializable]
    public struct BallEvent
    {
        public bool IsStay => power <= 0;
        public bool IsPush => power > 0;

        public int tick;
        public Vector3 position;
        public Vector3 direction;
        public float power;
    }
}