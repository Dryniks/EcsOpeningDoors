using System;
using System.Collections.Generic;
using EcsOpeningDoors.Component;
using Leopotam.EcsLite;

namespace EcsOpeningDoors
{
    public class World : IDisposable
    {
        private readonly EcsWorld _world;
        private readonly EcsSystems _systems;

        public World(IEnumerable<IEcsSystem> systems, TimeService timeService)
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world, timeService);

            InitPools();
            
            foreach (var system in systems)
                _systems.Add(system);

            _systems.Init();
        }

        //TODO Костыль, без него не инитятся пулы((((
        private void InitPools()
        {
            _world.GetPool<Acceleration>();
            _world.GetPool<AccelerationTo>();
            _world.GetPool<MovementRequest>();
            _world.GetPool<MovementSpeed>();
            _world.GetPool<MovementSpeedTo>();
            _world.GetPool<Position>();
            _world.GetPool<Rotation>();
            _world.GetPool<RotationRequest>();
            _world.GetPool<RotationSpeedTo>();
            _world.GetPool<Actor>();
            _world.GetPool<ClickPosition>();
            _world.GetPool<Scale>();
        }

        public Dictionary<int, List<object>> GetStartData(Dictionary<int, List<object>> dictionary)
        {
            var dict = new Dictionary<int, List<object>>();
            foreach (var value in dictionary.Values)
            {
                var entity = _world.NewEntity();

                if (!dict.ContainsKey(entity))
                    dict[entity] = new List<object>();

                foreach (var obj in value)
                {
                    var pool = _world.GetPoolByType(obj.GetType());
                    pool.AddRaw(entity, obj);

                    dict[entity].Add(obj);
                }
            }

            return dict;
        }

        public void Update()
        {
            _systems.Run();
        }

        public void AppendData(Type type, object data)
        {
            var entity = _world.NewEntity();
            var pool = _world.GetPoolByType(type);
            pool.AddRaw(entity, data);
        }

        public Dictionary<int, List<object>> GetData()
        {
            int[] entities = null;
            _world.GetAllEntities(ref entities);

            var dict = new Dictionary<int, List<object>>();

            IEcsPool[] pools = null;
            _world.GetAllPools(ref pools);

            foreach (var entity in entities)
            {
                dict.Add(entity, new List<object>());

                foreach (var pool in pools)
                {
                    if (pool.Has(entity))
                        dict[entity].Add(pool.GetRaw(entity));
                }
            }

            return dict;
        }

        public void Dispose()
        {
            _world.Destroy();
            _systems.Destroy();
        }
    }
}