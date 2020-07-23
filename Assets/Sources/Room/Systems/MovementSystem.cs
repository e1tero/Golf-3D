#pragma warning disable 649
using UnityEngine;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class MovementSystem: IEcsRunSystem
    {
        private readonly IRoom _room;
        private readonly HoleBehaviour _hole;
        private readonly EcsFilter<UnityObject, Movement>.Exclude<PushEvent> _items;

        public void Run()
        {
            foreach (var i in _items)
            {
                ref var ent = ref _items.GetEntity(i);
                ref var uObject = ref _items.Get1(i);
                var body = uObject.body;

                if (_items.Get2(i).time + 0.1f > Time.time) return;

                var pos = uObject.transform.position;
                if (pos.y < -0.2f)
                {
                    Debug.Log("DieEvent");
                    ent.Set<DieEvent>();
                    ent.Unset<Movement>();
                    continue;
                }

                var sqrMagnitude = body.velocity.sqrMagnitude;

                if (sqrMagnitude > 0.01f) continue;
                Debug.Log("StayEvent");
                body.velocity = Vector3.zero;
                body.useGravity = false;
                ent.Set<PlayerComponent>().lastStayPosition = body.position;
                ent.Set<StayEvent>().position = body.position;
                ent.Unset<Movement>();
            }
        }
    }
}