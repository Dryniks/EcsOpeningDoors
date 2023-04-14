using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System.Unity
{
    public class InputProcessingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _clickFilter;
        private EcsFilter _inputFilter;
        private EcsFilter _interruptionFilter;

        private EcsPool<MovementRequestComponent> _movementRequestsPool;
        private EcsPool<PositionComponent> _positionsPool;
        private EcsPool<RotationRequestComponent> _rotationRequestsPool;
        private EcsPool<InterruptionComponent> _interruptionsPool;
        private EcsPool<ClickPositionComponent> _clickPositionsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _inputFilter = world.Filter<CharacterComponent>().Inc<PositionComponent>().End();
            _interruptionFilter = world.Filter<CharacterComponent>().Inc<MovementRequestComponent>().End();
            _clickFilter = world.Filter<ClickPositionComponent>().End();

            _movementRequestsPool = world.GetPool<MovementRequestComponent>();
            _positionsPool = world.GetPool<PositionComponent>();
            _rotationRequestsPool = world.GetPool<RotationRequestComponent>();
            _interruptionsPool = world.GetPool<InterruptionComponent>();
            _clickPositionsPool = world.GetPool<ClickPositionComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var clickEntity in _clickFilter)
            {
                Debug.LogError("as");
                var clickPosition = _clickPositionsPool.Get(clickEntity).Value;
                Debug.LogError("as2");
                
                // foreach (var entity in _interruptionFilter)
                //     _interruptionsPool.Add(entity);

                foreach (var entity in _inputFilter)
                {
                    ref var movementRequest = ref _movementRequestsPool.GetOrAddComponent(entity);
                    movementRequest.Value = clickPosition;

                    var currentPosition = _positionsPool.Get(entity).Value;
                    var direction = (clickPosition - currentPosition).normalized;
                    var rotate = Quaternion.LookRotation(direction);

                    ref var rotationRequest = ref _rotationRequestsPool.GetOrAddComponent(entity);
                    rotationRequest.Value = rotate;
                }
                
                _clickPositionsPool.Del(clickEntity);
            }
        }
    }
}