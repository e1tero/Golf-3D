using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sources
{
    public class RoomService: IRoom
    {
        public int Id { get; private set; }
        public int Player { get; private set; }
        public int Tick { get; private set; }
        public bool Connected { get; private set; }
        public Ball[] Balls { get; private set; }

        private RoomListenRequest _listen;

        public void Join(int room)
        {
            new RoomJoinRequest()
            {
                id = room
            }.Send<RoomJoinResponse>(OnJoined, null);
        }

        public void Leave()
        {
            if (!Connected) return;
            new RoomLeaveRequest()
            {
                id = Id
            }.Send(null, null);
        }

        public void RegisterPush(Vector3 position, Vector3 direction, float power)
        {
            if (!Connected) return;
            Debug.Log("User ball impulse");
            new RoomPushRequest()
            {
                id = Id,
                player = Player,
                direction = direction,
                position = position,
                power = power
            }.Send();
        }

        public void RegisterStay(Vector3 position)
        {
            if (!Connected) return;
            Debug.Log("Stay");
            new RoomStayRequest()
            {
                id = Id,
                player = Player,
                position = position
            }.Send();
        }

        private void OnJoined(RoomJoinResponse response)
        {
            Id = response.id;
            Player = response.player;
            Tick = response.tick;
            Balls = response.balls;
            Connected = true;

            _listen = new RoomListenRequest()
            {
                id = Id,
                tick = Tick
            };
            _listen.Send<RoomListenResponse>(OnListen);
        }

        private void OnListen(RoomListenResponse response)
        {
            Balls = response.balls;
            Tick = response.tick;

            _listen.tick = response.tick;
            _listen.Send<RoomListenResponse>(OnListen);
        }
    }

    [Serializable]
    public struct RoomJoinRequest : IServiceRequest
    {
        public string Method => "ROOM_JOIN";
        public int id;
    }

    [Serializable]
    public struct RoomJoinResponse
    {
        public int id;
        public int player;
        public int tick;
        public Ball[] balls;
    }

    [Serializable]
    public struct RoomListenRequest : IServiceRequest
    {
        public string Method => "ROOM_LISTEN";
        public int id;
        public int tick;
    }

    [Serializable]
    public struct RoomListenResponse
    {
        public int id;
        public int tick;
        public Ball[] balls;
    }

    [Serializable]
    public struct RoomLeaveRequest : IServiceRequest
    {
        public string Method => "ROOM_LEAVE";
        public int id;
    }

    [Serializable]
    public struct RoomPushRequest : IServiceRequest
    {
        public string Method => "ROOM_PUSH";
        public int id;
        public int player;
        public Vector3 position;
        public Vector3 direction;
        public float power;
    }

    [Serializable]
    public struct RoomStayRequest : IServiceRequest
    {
        public string Method => "ROOM_STAY";
        public int id;
        public int player;
        public Vector3 position;
    }
}
