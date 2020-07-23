#pragma warning disable 649
using UnityEngine;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class PushSystem: IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly RoomDefinition _definition;
        private readonly EcsFilter<UnityObject, PlayerComponent, PushEvent>.Exclude<Movement, HoleComponent> _items;

        public void Run()
        {
            foreach (var i in _items)
            {
                ref var ent = ref _items.GetEntity(i);
                ref var uObject = ref _items.Get1(i);
                ref var player = ref _items.Get2(i);
                ref var evt = ref _items.Get3(i);
                var body = uObject.body;

                player.lastStayPosition = evt.position;
                body.position = evt.position;
                body.useGravity = true;

                body.AddForce(evt.direction.normalized * evt.power * _definition.BallMaxPower, ForceMode.Impulse);
                player.tick = evt.tick;
                ent.Set<Movement>().time = Time.time;
            }
        }
    }
}