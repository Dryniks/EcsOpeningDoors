using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class RotationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        private EcsPool<RotationRequest> _rotationRequestsPool;
        private EcsPool<Rotation> _rotationsPool;
        private EcsPool<RotationSpeedTo> _speedsToPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<RotationRequest>()
                .Inc<Rotation>()
                .Inc<RotationSpeedTo>()
                .End();

            _rotationRequestsPool = world.GetPool<RotationRequest>();
            _rotationsPool = world.GetPool<Rotation>();
            _speedsToPool = world.GetPool<RotationSpeedTo>();
        }

        public void Run(IEcsSystems systems)
        {
            var deltaTime = systems.GetShared<TimeService>().DeltaTime;

            foreach (var entity in _filter)
            {
                var speed = _speedsToPool.Get(entity).Value;
                var rotationRequest = _rotationRequestsPool.Get(entity);
                var t = deltaTime * speed;

                ref var rotation = ref _rotationsPool.Get(entity);
                rotation.Value = Quaternion.RotateTowards(rotation.Value, rotationRequest.Value, t);

                if (Quaternion.Angle(rotationRequest.Value, rotation.Value) > 0)
                    continue;

                _rotationRequestsPool.Del(entity);
            }
        }
    }
}