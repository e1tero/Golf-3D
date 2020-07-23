#pragma warning disable 649
using UnityEngine;
using System;
using Leopotam.Ecs;

namespace Sources
{
    public class CameraSystem: IEcsRunSystem
    {
        private readonly CameraBehaviour _camera;
        private readonly HoleBehaviour _hole;
        private readonly EcsFilter<UnityObject, LocalPlayer> _items;

        public void Run()
        {
            foreach (var i in _items)
            {
                ref var uObj = ref _items.Get1(i);
                LookAt(uObj.transform.position);
            }
        }

        private void LookAt(Vector3 position)
        {
            _camera.transform.position = position;
            _camera.transform.LookAt(_hole.transform);
        }
    }
}