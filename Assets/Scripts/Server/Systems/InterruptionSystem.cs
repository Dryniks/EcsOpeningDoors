using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System
{
    public class InterruptionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _clickFilter;
        private EcsFilter _accelerationFilter;
        private EcsFilter _speedFilter;

        private EcsPool<Acceleration> _accelerationsPool;
        private EcsPool<MovementSpeed> _movementSpeedsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _accelerationFilter = world.Filter<Actor>().Inc<Acceleration>().End();
            _speedFilter = world.Filter<Actor>().Inc<MovementSpeed>().End();
            _clickFilter = world.Filter<ClickPosition>().End();

            _movementSpeedsPool = world.GetPool<MovementSpeed>();
            _accelerationsPool = world.GetPool<Acceleration>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_clickFilter.GetEntitiesCount() == 0)
                return;

            foreach (var entity in _accelerationFilter)
                _accelerationsPool.Del(entity);

            foreach (var entity in _speedFilter)
                _movementSpeedsPool.Del(entity);
        }
    }
}