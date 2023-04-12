using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class MovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly TimeService _timeService;

        private EcsFilter _filter;

        private EcsPool<MovableViewComponent> _viewsPool;
        private EcsPool<MovementRequestComponent> _movementRequestsPool;
        private EcsPool<PositionComponent> _positionsPool;
        private EcsPool<MovementSpeedComponent> _speedsPool;

        public MovementSystem(TimeService timeService)
        {
            _timeService = timeService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovableViewComponent>()
                .Inc<MovementRequestComponent>()
                .Inc<PositionComponent>()
                .Inc<MovementSpeedComponent>()
                .Exc<InterruptionComponent>()
                .Exc<MovementCompletionComponent>()
                .End();

            _viewsPool = world.GetPool<MovableViewComponent>();
            _movementRequestsPool = world.GetPool<MovementRequestComponent>();
            _positionsPool = world.GetPool<PositionComponent>();
            _speedsPool = world.GetPool<MovementSpeedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var movementRequest = _movementRequestsPool.Get(entity);
                var speed = _speedsPool.Get(entity).Value;
                var view = _viewsPool.Get(entity).View;

                var maxDistanceDelta = _timeService.FixedDeltaTime * speed;

                ref var position = ref _positionsPool.Get(entity);
                position.Value = Vector3.MoveTowards(position.Value, movementRequest.Value, maxDistanceDelta);

                view.SetPosition(position.Value);
            }
        }
    }
}