using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class CameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _actorFilter;
        private EcsFilter _cameraFilter;

        private EcsPool<CameraComponent> _camerasPool;
        private EcsPool<Position> _positionsPool;
        private EcsPool<MovementRequest> _movementRequestsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _actorFilter = world.Filter<Actor>()
                .Inc<Position>()
                .Inc<MovementRequest>()
                .End();

            _cameraFilter = world.Filter<CameraComponent>()
                .Inc<Position>()
                .End();

            _camerasPool = world.GetPool<CameraComponent>();
            _positionsPool = world.GetPool<Position>();
            _movementRequestsPool = world.GetPool<MovementRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var actorEntity in _actorFilter)
            {
                var actorPosition = _positionsPool.Get(actorEntity);

                foreach (var cameraEntity in _cameraFilter)
                {
                    var camera = _camerasPool.Get(cameraEntity);
                    ref var position = ref _movementRequestsPool.GetOrAddComponent(cameraEntity);
                    position.Value = actorPosition.Value + camera.Offset;
                }
            }
        }
    }
}