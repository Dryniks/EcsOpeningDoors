using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class CleaningSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _movementCompletionFilter;
        private EcsFilter _rotationCompletionFilter;
        private EcsFilter _accelerationFilter;

        private EcsPool<RotationCompletionComponent> _rotationCompletionsPool;
        private EcsPool<MovementCompletionComponent> _movementCompletionsPool;
        private EcsPool<RotationRequestComponent> _rotationRequestsPool;
        private EcsPool<MovementRequestComponent> _movementRequestsPool;
        private EcsPool<AccelerationComponent> _accelerationsPool;
        private EcsPool<MovementSpeedComponent> _movementSpeedsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _movementCompletionFilter = world.Filter<ActorComponent>().Inc<MovementCompletionComponent>().End();
            _rotationCompletionFilter = world.Filter<ActorComponent>().Inc<RotationCompletionComponent>().End();
            _accelerationFilter = world.Filter<ActorComponent>().Inc<MovementCompletionComponent>().Inc<AccelerationComponent>().End();

            _rotationCompletionsPool = world.GetPool<RotationCompletionComponent>();
            _movementCompletionsPool = world.GetPool<MovementCompletionComponent>();
            _rotationRequestsPool = world.GetPool<RotationRequestComponent>();
            _movementRequestsPool = world.GetPool<MovementRequestComponent>();
            _accelerationsPool = world.GetPool<AccelerationComponent>();
            _movementSpeedsPool = world.GetPool<MovementSpeedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _accelerationFilter)
            {
                _movementSpeedsPool.Del(entity);
                _accelerationsPool.Del(entity);
            }

            foreach (var entity in _movementCompletionFilter)
            {
                _movementRequestsPool.Del(entity);
                _movementCompletionsPool.Del(entity);
            }

            foreach (var entity in _rotationCompletionFilter)
            {
                _rotationRequestsPool.Del(entity);
                _rotationCompletionsPool.Del(entity);
            }
        }
    }
}