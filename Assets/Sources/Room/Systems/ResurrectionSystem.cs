#pragma warning disable 649
using UnityEngine;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class ResurrectionSystem: IEcsRunSystem
    {
        private readonly EcsFilter<UnityObject, PlayerComponent, DieEvent> _items;

        public void Run()
        {
            foreach (var i in _items)
            {
                ref var ent = ref _items.GetEntity(i);
                ref var unity = ref _items.Get1(i);
                ref var player = ref _items.Get2(i);

                unity.body.velocity = Vector3.zero;
                unity.body.position = player.lastStayPosition;
                unity.body.useGravity = false;
                ent.Unset<DieEvent>();
                ent.Set<StayEvent>().position = player.lastStayPosition;
                Debug.Log("Resurrect");
            }
        }
    }
}