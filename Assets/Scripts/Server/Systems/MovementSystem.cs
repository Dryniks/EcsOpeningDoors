using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class MovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        private EcsPool<MovementRequest> _movementRequestsPool;
        private EcsPool<Position> _positionsPool;
        private EcsPool<MovementSpeed> _speedsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovementRequest>()
                .Inc<Position>()
                .Inc<MovementSpeed>()
                .Exc<RotationRequest>()
                .End();

            _movementRequestsPool = world.GetPool<MovementRequest>();
            _positionsPool = world.GetPool<Position>();
            _speedsPool = world.GetPool<MovementSpeed>();
        }

        public void Run(IEcsSystems systems)
        {
            var deltaTime = systems.GetShared<TimeService>().DeltaTime;

            foreach (var entity in _filter)
            {
                var movementRequest = _movementRequestsPool.Get(entity);
                var speed = _speedsPool.Get(entity);

                var maxDistanceDelta = deltaTime * speed.Value;

                ref var position = ref _positionsPool.Get(entity);
                position.Value = Vector3.MoveTowards(position.Value, movementRequest.Value, maxDistanceDelta);

                if (!Mathf.Approximately(Vector3.Distance(position.Value, movementRequest.Value), 0.0f))
                    continue;
                
                _movementRequestsPool.Del(entity);
            }
        }
    }
}