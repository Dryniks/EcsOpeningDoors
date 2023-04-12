using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class AccelerationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly TimeService _timeService;

        private EcsFilter _filter;

        private EcsPool<MovementSpeedComponent> _speedsPool;
        private EcsPool<MovementSpeedToComponent> _speedsToPool;

        private EcsPool<AccelerationComponent> _accelerationsPool;
        private EcsPool<AccelerationToComponent> _accelerationsToPool;

        public AccelerationSystem(TimeService timeService)
        {
            _timeService = timeService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<AccelerationToComponent>()
                .Inc<MovementRequestComponent>()
                .Inc<MovementSpeedToComponent>()
                .Exc<InterruptionComponent>()
                .Exc<MovementCompletionComponent>()
                .End();

            _accelerationsPool = world.GetPool<AccelerationComponent>();
            _accelerationsToPool = world.GetPool<AccelerationToComponent>();

            _speedsPool = world.GetPool<MovementSpeedComponent>();
            _speedsToPool = world.GetPool<MovementSpeedToComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var accelerationTo = _accelerationsToPool.Get(entity);

                ref var acceleration = ref _accelerationsPool.GetOrAddComponent(entity);
                acceleration.Value += _timeService.DeltaTime;

                var t = Mathf.InverseLerp(0, accelerationTo.Value, acceleration.Value);

                var speedTo = _speedsToPool.Get(entity);

                ref var speed = ref _speedsPool.GetOrAddComponent(entity);
                speed.Value = Mathf.Lerp(0, speedTo.Value, t);
            }
        }
    }
}