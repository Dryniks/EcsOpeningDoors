using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class CheckMovementCompletionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float MinDistance = 0.1f;

        private EcsFilter _filter;

        private EcsPool<MovementRequestComponent> _movementRequestsPool;
        private EcsPool<MovementCompletionComponent> _movementCompletionsPool;
        private EcsPool<PositionComponent> _positionsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovementRequestComponent>()
                .Inc<PositionComponent>()
                .Exc<InterruptionComponent>()
                .Exc<MovementCompletionComponent>()
                .End();

            _movementRequestsPool = world.GetPool<MovementRequestComponent>();
            _movementCompletionsPool = world.GetPool<MovementCompletionComponent>();
            _positionsPool = world.GetPool<PositionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var movementRequest = _movementRequestsPool.Get(entity);
                var position = _positionsPool.Get(entity);

                if (Vector3.Distance(position.Value, movementRequest.Value) >= MinDistance)
                    continue;

                _movementCompletionsPool.Add(entity);
            }
        }
    }
}