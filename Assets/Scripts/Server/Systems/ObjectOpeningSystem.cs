using EcsOpeningDoors.Component;
using Leopotam.EcsLite;

namespace Server.Systems
{
    public class ObjectOpeningSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _openSignalFilter;
        private EcsFilter _doorFilter;

        private EcsPool<Link> _linksPool;
        private EcsPool<MovementRequest> _movementRequestsPool;
        private EcsPool<Door> _doorsPool;
        private EcsPool<OpenSignal> _openSignalsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _openSignalFilter = world.Filter<OpenSignal>().End();
            _doorFilter = world.Filter<Door>().End();

            _linksPool = world.GetPool<Link>();
            _movementRequestsPool = world.GetPool<MovementRequest>();
            _doorsPool = world.GetPool<Door>();
            _openSignalsPool = world.GetPool<OpenSignal>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_openSignalFilter.GetEntitiesCount() != 0)
                StartOpen(systems);
            else
                StopOpen();
        }

        private void StartOpen(IEcsSystems systems)
        {
            foreach (var openSignalEntity in _openSignalFilter)
            {
                var pressId = _linksPool.Get(openSignalEntity).Id;

                foreach (var doorEntity in _doorFilter)
                {
                    var doorId = _linksPool.Get(doorEntity).Id;
                    if (!string.Equals(pressId, doorId))
                        continue;

                    var targetPosition = _doorsPool.Get(doorEntity).OpenPoint;

                    ref var request = ref _movementRequestsPool.GetOrAddComponent(doorEntity);
                    request.Value = targetPosition;
                }

                _linksPool.Del(openSignalEntity);
                _openSignalsPool.Del(openSignalEntity);

                var world = systems.GetWorld();
                world.DelEntity(openSignalEntity);
            }
        }

        private void StopOpen()
        {
            foreach (var entity in _doorFilter)
            {
                if (_movementRequestsPool.Has(entity))
                    _movementRequestsPool.Del(entity);
            }
        }
    }
}