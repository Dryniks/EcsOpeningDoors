using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class AccelerationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _resetFilter;

        private EcsPool<MovementSpeed> _speedsPool;
        private EcsPool<MovementSpeedTo> _speedsToPool;

        private EcsPool<Acceleration> _accelerationsPool;
        private EcsPool<AccelerationTo> _accelerationsToPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<AccelerationTo>()
                .Inc<MovementRequest>()
                .Inc<MovementSpeedTo>()
                .Exc<RotationRequest>()
                .End();

            _resetFilter = world.Filter<AccelerationTo>()
                .Inc<MovementSpeedTo>()
                .Exc<RotationRequest>()
                .Exc<MovementRequest>()
                .End();

            _accelerationsPool = world.GetPool<Acceleration>();
            _accelerationsToPool = world.GetPool<AccelerationTo>();

            _speedsPool = world.GetPool<MovementSpeed>();
            _speedsToPool = world.GetPool<MovementSpeedTo>();
        }

        public void Run(IEcsSystems systems)
        {
            var deltaTime = systems.GetShared<TimeService>().DeltaTime;

            if (_filter.GetEntitiesCount() != 0)
                IncreaseSpeed(deltaTime);
            else
                ResetSpeed();
        }

        private void IncreaseSpeed(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var accelerationTo = _accelerationsToPool.Get(entity);

                ref var acceleration = ref _accelerationsPool.GetOrAddComponent(entity);
                acceleration.Value += deltaTime;

                var t = Mathf.InverseLerp(0, accelerationTo.Value, acceleration.Value);

                var speedTo = _speedsToPool.Get(entity);

                ref var speed = ref _speedsPool.GetOrAddComponent(entity);
                speed.Value = Mathf.Lerp(0, speedTo.Value, t);
            }
        }

        private void ResetSpeed()
        {
            foreach (var entity in _resetFilter)
            {
                _speedsPool.Del(entity);
                _accelerationsPool.Del(entity);
            }
        }
    }
}