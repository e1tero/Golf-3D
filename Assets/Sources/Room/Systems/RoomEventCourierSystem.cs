#pragma warning disable 649
using UnityEngine;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class RoomEventCourierSystem: IEcsRunSystem
    {
        private readonly IRoom _room;
        private readonly EcsFilter<PlayerComponent, RoomEventContainer, NetworkPlayer>.Exclude<Movement> _players;

        public void Run()
        {
            foreach (var i in _players)
            {
                ref var ent = ref _players.GetEntity(i);
                ref var player = ref _players.Get1(i);
                ref var container = ref _players.Get2(i);

                if (container.events == null) continue;

                for (int e = 0; e < container.events.Length; e++)
                {
                    ref var evt = ref container.events[e];
                    if (evt.tick > player.tick)
                    {
                        player.tick = evt.tick;
                        if (evt.IsStay)
                        {
                            //ent.Set<UnityObject>().body.position = evt.position;
                            continue;
                        }

                        ref var push = ref ent.Set<PushEvent>();
                        push.tick = evt.tick;
                        push.position = evt.position;
                        push.direction = evt.direction;
                        push.power = evt.power;
                    }
                }
            }
        }
    }
}