#pragma warning disable 649
using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Sources {
    sealed class RoomStartup : MonoBehaviour
    {
        public RoomDefinition _definition;

        EcsWorld _world;
        EcsSystems _systems;

        IEnumerator Start ()
        {
            yield return null;
            yield return null;
            yield return null;

            _world = new EcsWorld ();
            _systems = new EcsSystems (_world, "room");
#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
            _systems
                .Add(new RoomSystem())
                .Add(new RoomEventCourierSystem())
                .Add(new MovementSystem())
                .Add(new ResurrectionSystem())
                .Add(new JoystickInputSystem())
                .Add(new PushSystem())
                .Add(new CameraSystem())
                .Add(new RoomEventReceiverSystem());

            // Events
            _systems
                .OneFrame<PushEvent>()
                .OneFrame<StayEvent>();

            _systems
                .Inject (_definition)
                .Inject(Game.JoystickBehaviour)
                .Inject(Game.CameraBehaviour)
                .Inject(Game.TeeBehaviour)
                .Inject(Game.HoleBehaviour)
                .Inject(new RoomService())
                .ProcessInjects()
                .Init();

            Game.room = new Room(_world);
        }

        void Update () {
            _systems?.Run();
        }

        void OnDestroy () {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }
    }

    public struct Room
    {
        public readonly EcsWorld world;

        public Room(EcsWorld world)
        {
            this.world = world;
        }
    }
}