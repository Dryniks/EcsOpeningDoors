using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class InterruptionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _interruptionFilter;
        private EcsFilter _rotationCompletionFilter;
        private EcsFilter _movementCompletionFilter;
        private EcsFilter _accelerationFilter;
        private EcsFilter _movementSpeedFilter;

        private EcsPool<InterruptionComponent> _interruptionsPool;
        private EcsPool<RotationCompletionComponent> _rotationCompletionsPool;
        private EcsPool<MovementCompletionComponent> _movementCompletionsPool;
        private EcsPool<AccelerationComponent> _accelerationsPool;
        private EcsPool<MovementSpeedComponent> _movementSpeedsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _interruptionFilter = world.Filter<CharacterComponent>().Inc<InterruptionComponent>().End();
            _rotationCompletionFilter = world.Filter<CharacterComponent>().Inc<InterruptionComponent>().Inc<RotationCompletionComponent>().End();
            _movementCompletionFilter = world.Filter<CharacterComponent>().Inc<InterruptionComponent>().Inc<MovementCompletionComponent>().End();
            _accelerationFilter = world.Filter<CharacterComponent>().Inc<InterruptionComponent>().Inc<AccelerationComponent>().End();
            _movementSpeedFilter = world.Filter<CharacterComponent>().Inc<InterruptionComponent>().Inc<MovementSpeedComponent>().End();

            _interruptionsPool = world.GetPool<InterruptionComponent>();
            _rotationCompletionsPool = world.GetPool<RotationCompletionComponent>();
            _movementCompletionsPool = world.GetPool<MovementCompletionComponent>();
            _accelerationsPool = world.GetPool<AccelerationComponent>();
            _movementSpeedsPool = world.GetPool<MovementSpeedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _rotationCompletionFilter)
                _rotationCompletionsPool.Del(entity);

            foreach (var entity in _movementCompletionFilter)
                _movementCompletionsPool.Del(entity);

            foreach (var entity in _accelerationFilter)
                _accelerationsPool.Del(entity);

            foreach (var entity in _movementSpeedFilter)
                _movementSpeedsPool.Del(entity);

            foreach (var entity in _interruptionFilter)
                _interruptionsPool.Del(entity);
        }
    }
}