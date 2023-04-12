using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace EcsOpeningDoors.System.Unity
{
    public class MovementAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _changeSpeedFilter;
        private EcsFilter _startRunFilter;
        private EcsFilter _endRunFilter;

        private EcsPool<ActorComponent> _actorsPool;
        private EcsPool<MovementSpeedComponent> _speedsPool;
        private EcsPool<MovementSpeedToComponent> _speedsToPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _changeSpeedFilter = world.Filter<ActorComponent>().Inc<MovementSpeedComponent>().End();

            _startRunFilter = world.Filter<ActorComponent>().Inc<RotationCompletionComponent>().End();
            _endRunFilter = world.Filter<ActorComponent>().Inc<MovementCompletionComponent>().End();

            _actorsPool = world.GetPool<ActorComponent>();
            _speedsPool = world.GetPool<MovementSpeedComponent>();
            _speedsToPool = world.GetPool<MovementSpeedToComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _startRunFilter)
            {
                var actor = _actorsPool.Get(entity).Actor;

                actor.SetMovementSpeed(0);
                actor.StartRun();
            }

            if (_endRunFilter.GetEntitiesCount() == 0)
                ChangeAnimationSpeed();

            foreach (var entity in _endRunFilter)
                _actorsPool.Get(entity).Actor.EndRun();
        }

        private void ChangeAnimationSpeed()
        {
            foreach (var entity in _changeSpeedFilter)
            {
                var actor = _actorsPool.Get(entity).Actor;
                var speed = _speedsPool.Get(entity);
                var speedTo = _speedsToPool.Get(entity);

                var animationSpeed = Mathf.InverseLerp(0, speedTo.Value, speed.Value);

                actor.SetMovementSpeed(animationSpeed);
            }
        }
    }
}