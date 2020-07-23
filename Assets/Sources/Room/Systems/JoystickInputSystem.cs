#pragma warning disable 649
using UnityEngine;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class JoystickInputSystem: IEcsRunSystem
    {
        private readonly EcsFilter<LocalPlayer>.Exclude<Movement> _items;
        private readonly EcsFilter<LocalPlayer, StayEvent> _ready;
        private readonly JoystickBehaviour _joystick;
        public void Run()
        {
            if (_ready.GetEntitiesCount() > 0)
                _joystick.SetReady();

            if (!_joystick.Pushed) return;

            foreach (var i in _items)
            {
                ref var ent = ref _items.GetEntity(i);
                ref var evt = ref ent.Set<PushEvent>();

                evt.position = ent.Set<UnityObject>().body.position;
                evt.direction = _joystick.Direction;
                evt.power = _joystick.Power;
            }

            _joystick.Pushed = false;
        }
    }
}