using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class CheckRotationCompletionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        private EcsPool<RotationComponent> _rotationsPool;
        private EcsPool<RotationRequestComponent> _rotationRequestsPool;
        private EcsPool<RotationCompletionComponent> _rotationCompletionsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<RotationRequestComponent>()
                .Inc<RotationComponent>()
                .Exc<InterruptionComponent>()
                .Exc<RotationCompletionComponent>()
                .End();

            _rotationsPool = world.GetPool<RotationComponent>();
            _rotationRequestsPool = world.GetPool<RotationRequestComponent>();
            _rotationCompletionsPool = world.GetPool<RotationCompletionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var rotationRequest = _rotationRequestsPool.Get(entity);
                var rotation = _rotationsPool.Get(entity);

                if (Quaternion.Angle(rotation.Value, rotationRequest.Value) >= float.Epsilon)
                    continue;
                
                _rotationCompletionsPool.Add(entity);
            }
        }
    }
}