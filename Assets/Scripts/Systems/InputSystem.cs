using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IInputController _inputController;

        private EcsFilter _inputFilter;
        private EcsFilter _interruptionFilter;

        private EcsPool<MovementRequestComponent> _movementRequestsPool;
        private EcsPool<PositionComponent> _positionsPool;
        private EcsPool<RotationRequestComponent> _rotationRequestsPool;
        private EcsPool<InterruptionComponent> _interruptionsPool;

        public InputSystem(IInputController inputController)
        {
            _inputController = inputController;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _inputFilter = world.Filter<CharacterComponent>().Inc<PositionComponent>().End();
            _interruptionFilter = world.Filter<CharacterComponent>().Inc<MovementRequestComponent>().End();

            _movementRequestsPool = world.GetPool<MovementRequestComponent>();
            _positionsPool = world.GetPool<PositionComponent>();
            _rotationRequestsPool = world.GetPool<RotationRequestComponent>();
            _interruptionsPool = world.GetPool<InterruptionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var targetPosition = _inputController.GetPosition();
            if (!targetPosition.HasValue)
                return;

            var positionValue = targetPosition.Value;

            foreach (var entity in _interruptionFilter)
                _interruptionsPool.Add(entity);

            foreach (var entity in _inputFilter)
            {
                ref var movementRequest = ref _movementRequestsPool.GetOrAddComponent(entity);
                movementRequest.Value = positionValue;

                var currentPosition = _positionsPool.Get(entity).Value;
                var direction = (positionValue - currentPosition).normalized;
                var rotate = Quaternion.LookRotation(direction);

                ref var rotationRequest = ref _rotationRequestsPool.GetOrAddComponent(entity);
                rotationRequest.Value = rotate;
            }
        }
    }
}