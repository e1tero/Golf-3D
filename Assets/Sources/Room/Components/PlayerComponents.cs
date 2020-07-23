using UnityEngine;

namespace Sources
{
    public struct PlayerComponent
    {
        public int id;
        public int tick;
        public Vector3 lastStayPosition;
    }

    public struct Movement
    {
        public float time;
    }
    public struct LocalPlayer { }
    public struct NetworkPlayer { }

    public struct RoomEventContainer
    {
        public BallEvent[] events;
    }

}