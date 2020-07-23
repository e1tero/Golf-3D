#pragma warning disable 649
using UnityEngine;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class RoomEventReceiverSystem: IEcsRunSystem
    {
        private readonly IRoom _room;
        private readonly EcsFilter<PushEvent, PlayerComponent, LocalPlayer> _pushEvents;
        private readonly EcsFilter<StayEvent, PlayerComponent, LocalPlayer> _stayEvents;

        public void Run()
        {
            foreach (var i in _pushEvents)
            {
                ref var e = ref _pushEvents.Get1(i);
                _room.RegisterPush(e.position, e.direction, e.power);
            }

            foreach (var i in _stayEvents)
            {
                ref var e = ref _stayEvents.Get1(i);
                _room.RegisterStay(e.position);
            }
        }
    }
}