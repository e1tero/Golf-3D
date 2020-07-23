#pragma warning disable 649
using UnityEngine;
using Leopotam.Ecs;

namespace Sources
{
    public class RoomSystem: IEcsInitSystem, IEcsRunSystem
    {
        private enum BallState
        {
            unknown,
            create,
            delete,
            sync
        }

        private readonly RoomDefinition _definition;
        private readonly TeeBehaviour _tee;
        private readonly IRoom _room;
        private readonly EcsWorld _world;
        private readonly EcsFilter<UnityObject, PlayerComponent, HoleComponent> _items;
        private readonly EcsFilter<PlayerComponent> _players;
        private BallState[] _states = new BallState[12];
        
        public void Init()
        {
            _room.Join(1);

            var ent = _world.NewEntity();
            ent.Set<RoomComponent>().id = 1;
        }

        public void Run()
        {
            foreach (var i in _items)
            {
                ref var ent = ref _items.GetEntity(i);
                ref var unity = ref _items.Get1(i);
                unity.gameObject.SetActive(false);

                if (ent.Has<LocalPlayer>())
                {
                    Game.UI.Show<ScreenLoading>().ExitRoom();
                }
            }

            if (!_room.Connected || _room?.Balls == null) return;

            if (_states.Length < _room.Balls.Length)
                _states = new BallState[_room.Balls.Length * 2];

            foreach (var i in _players)
            {
                ref var ent = ref _players.GetEntity(i);
                ref var player = ref _players.Get1(i);
                bool sync = false;

                for (var b = 0; b < _room.Balls.Length; b++)
                {
                    var ball = _room.Balls[b];
                    _states[b] = BallState.sync;
                    if (ball.player == player.id)
                    {
                        ref var container = ref ent.Set<RoomEventContainer>();
                        container.events = ball.events;
                        sync = true;
                        break;
                    }
                    _states[b] = BallState.create;
                }

                if (!sync)
                {
                    Pool.Release(ent.Set<UnityObject>().gameObject);
                    ent.Destroy();
                }
            }

            for (int b = _room.Balls.Length - 1; b >= 0; b--)
            {
                if (_states[b] == BallState.sync) continue;
                var ball = _room.Balls[b];
                var ent = SpawnBall(ball.player);
                
                if (ball.player == _room.Player)
                    ent.Set<LocalPlayer>();
                else
                {
                    ent.Set<NetworkPlayer>();
                    ref var container = ref ent.Set<RoomEventContainer>();
                    container.events = ball.events;
                }
            }
        }
        
        private EcsEntity SpawnBall(int id)
        {
            Debug.Log("Spawn ball");
            var go = Pool.Spawn(_definition.PlayerPrefab);
            var ent = _world.NewEntity();
            ref var uObject = ref ent.Set<UnityObject>();
            var uEnt = go.GetComponent<EntityBehaviour>().entity = ent;
            uObject.gameObject = go;
            uObject.transform = go.transform;
            uObject.body = go.GetComponent<Rigidbody>();
            uObject.body.velocity = Vector3.zero;
            uObject.body.position = _tee.GetPosition(id);
            var renderer = uObject.gameObject.GetComponent<MeshRenderer>();
            renderer.material = _definition.ballMaterials[Random.Range(0, _definition.ballMaterials.Length)];
            ent.Set<PlayerComponent>().id = id;
            ent.Set<StayEvent>().position = uObject.body.position;
            return ent;
        }
    }
}