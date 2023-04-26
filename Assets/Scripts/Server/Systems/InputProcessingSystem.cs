using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System.Unity
{
    public class InputProcessingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _clickFilter;
        private EcsFilter _actorFilter;

        private EcsPool<MovementRequest> _movementRequestsPool;
        private EcsPool<Position> _positionsPool;
        private EcsPool<RotationRequest> _rotationRequestsPool;
        private EcsPool<ClickPosition> _clickPositionsPool;
        private EcsPool<MovingObject> _movingObjectsPool;
        private EcsPool<RotatingObject> _rotatingObjectsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _actorFilter = world.Filter<Actor>().Inc<Position>().End();
            _clickFilter = world.Filter<ClickPosition>().End();

            _movementRequestsPool = world.GetPool<MovementRequest>();
            _positionsPool = world.GetPool<Position>();
            _rotationRequestsPool = world.GetPool<RotationRequest>();
            _clickPositionsPool = world.GetPool<ClickPosition>();
            _movingObjectsPool = world.GetPool<MovingObject>();
            _rotatingObjectsPool = world.GetPool<RotatingObject>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var clickEntity in _clickFilter)
            {
                var clickPosition = _clickPositionsPool.Get(clickEntity).Value;

                foreach (var actorEntity in _actorFilter)
                {
                    if (_movingObjectsPool.Has(actorEntity))
                    {
                        ref var movementRequest = ref _movementRequestsPool.GetOrAddComponent(actorEntity);
                        movementRequest.Value = clickPosition;
                    }

                    if (_rotatingObjectsPool.Has(actorEntity))
                    {
                        var currentPosition = _positionsPool.Get(actorEntity).Value;
                        var direction = (clickPosition - currentPosition).normalized;
                        var rotate = Quaternion.LookRotation(direction);

                        ref var rotationRequest = ref _rotationRequestsPool.GetOrAddComponent(actorEntity);
                        rotationRequest.Value = rotate;
                    }
                }

                _clickPositionsPool.Del(clickEntity);

                var world = systems.GetWorld();
                world.DelEntity(clickEntity);
            }
        }
    }
}