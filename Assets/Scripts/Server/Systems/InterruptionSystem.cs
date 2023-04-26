using Leopotam.EcsLite;
using EcsOpeningDoors.Component;
using UnityEngine;

namespace EcsOpeningDoors.System
{
    public class InterruptionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float AccelerationAngle = 45;

        private EcsFilter _clickFilter;
        private EcsFilter _actorFilter;

        private EcsPool<Acceleration> _accelerationsPool;
        private EcsPool<MovementSpeed> _movementSpeedsPool;
        private EcsPool<Position> _positionsPool;
        private EcsPool<Rotation> _rotationsPool;
        private EcsPool<ClickPosition> _clickPositionsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _actorFilter = world.Filter<Actor>()
                .Inc<MovementSpeed>()
                .Inc<Position>()
                .End();
            _clickFilter = world.Filter<ClickPosition>().End();

            _movementSpeedsPool = world.GetPool<MovementSpeed>();
            _accelerationsPool = world.GetPool<Acceleration>();
            _positionsPool = world.GetPool<Position>();
            _rotationsPool = world.GetPool<Rotation>();
            _clickPositionsPool = world.GetPool<ClickPosition>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var clickEntity in _clickFilter)
            {
                var clickPosition = _clickPositionsPool.Get(clickEntity).Value;

                foreach (var actorEntity in _actorFilter)
                {
                    var actorPosition = _positionsPool.Get(actorEntity).Value;
                    var actorRotation = _rotationsPool.Get(actorEntity).Value;

                    var direction = (clickPosition - actorPosition).normalized;
                    var rotate = Quaternion.LookRotation(direction);

                    var angle = Quaternion.Angle(rotate, actorRotation);
                    if (angle < AccelerationAngle)
                        continue;

                    if (!_accelerationsPool.Has(actorEntity))
                        continue;

                    _movementSpeedsPool.Del(actorEntity);
                    _accelerationsPool.Del(actorEntity);
                }
            }
        }
    }
}