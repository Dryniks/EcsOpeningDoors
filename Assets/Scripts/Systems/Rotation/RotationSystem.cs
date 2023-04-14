using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class RotationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        private EcsPool<RotatableViewComponent> _viewsPool;
        private EcsPool<RotationRequestComponent> _rotationRequestsPool;
        private EcsPool<RotationComponent> _rotationsPool;
        private EcsPool<RotationSpeedComponent> _speedsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<RotatableViewComponent>()
                .Inc<RotationRequestComponent>()
                .Inc<RotationComponent>()
                .Inc<RotationSpeedComponent>()
                .Exc<InterruptionComponent>()
                .Exc<RotationCompletionComponent>()
                .End();

            _viewsPool = world.GetPool<RotatableViewComponent>();
            _rotationRequestsPool = world.GetPool<RotationRequestComponent>();
            _rotationsPool = world.GetPool<RotationComponent>();
            _speedsPool = world.GetPool<RotationSpeedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var deltaTime = systems.GetShared<TimeService>().DeltaTime;

            foreach (var entity in _filter)
            {
                var speed = _speedsPool.Get(entity).Value;
                var rotationRequest = _rotationRequestsPool.Get(entity);
                var view = _viewsPool.Get(entity).View;
                var t = deltaTime * speed;

                ref var rotation = ref _rotationsPool.Get(entity);
                rotation.Value = Quaternion.RotateTowards(rotation.Value, rotationRequest.Value, t);

                view.SetRotation(rotation.Value);
            }
        }
    }
}