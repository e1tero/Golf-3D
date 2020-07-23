#pragma warning disable 649
using UnityEngine;
using System;

namespace Sources
{
    public class RoomLocal: IRoom
    {
        public int Id { get; private set; }
        public int Player { get; private set; }
        public int Tick { get; private set; }
        public bool Connected { get; private set; }
        public Ball[] Balls { get; private set; }

        public void Join(int id)
        {
            Id = id;
            Player = 1;
            Balls = new Ball[1];
            Balls[0] = new Ball()
            {
                player = 1
            };

            //Balls[1] = new Ball()
            //{
            //    player = 2,
            //    events = new BallEvent[1]
            //};

            Connected = true;
        }

        public void Leave()
        {

        }

        public void RegisterStay(Vector3 position)
        {
            Debug.Log("Stay");
            return;
            ref var e = ref Balls[1].events[0];
            e.position = position;
            e.power = 0;
            e.tick++;
        }

        public void RegisterPush(Vector3 position, Vector3 direction, float power)
        {
            Debug.Log("Push");
            return;
            ref var e = ref Balls[1].events[0];
            e.position = position;
            e.power = power;
            e.direction = direction;
            e.tick++;
        }
    }
}