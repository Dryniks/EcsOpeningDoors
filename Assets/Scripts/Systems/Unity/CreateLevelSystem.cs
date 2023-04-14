using UnityEngine;
using Leopotam.EcsLite;
using EcsOpeningDoors.Component;
using EcsOpeningDoors.Unity;

namespace EcsOpeningDoors.System.Unity
{
    public class CreateLevelSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var objectViews = Object.FindObjectsOfType<ObjectView>();
            foreach (var view in objectViews)
            {
                var actor = view.GetComponent<ActorView>();
                if (actor != null)
                    InitCharacterComponents(world, actor);
            }
        }

        private static void InitCharacterComponents(EcsWorld world, ActorView view)
        {
            var transform = view.transform;
            var entity = world.NewEntity();

            ref var actor = ref world.GetPool<ActorComponent>().Add(entity);
            actor.Actor = view;

            world.GetPool<CharacterComponent>().Add(entity);

            InitMovableComponents(world, transform, entity);
            InitRotatableComponents(world, transform, entity);
            InitAccelerationToComponent(world, transform, entity);
        }

        private static void InitRotatableComponents(EcsWorld world, UnityEngine.Component component, int entity)
        {
            var rotatableView = component.GetComponent<RotatingObject>();
            if (rotatableView == null)
                return;

            var rotationViewsPool = world.GetPool<RotatableViewComponent>();

            ref var rotationViewComponent = ref rotationViewsPool.Add(entity);
            rotationViewComponent.View = rotatableView;

            var rotationSpeedsPool = world.GetPool<RotationSpeedComponent>();

            ref var rotationSpeedComponent = ref rotationSpeedsPool.Add(entity);
            rotationSpeedComponent.Value = rotatableView.Speed;

            var rotationsPool = world.GetPool<RotationComponent>();
            ref var rotationComponent = ref rotationsPool.Add(entity);
            rotationComponent.Value = component.transform.rotation;
        }

        private static void InitMovableComponents(EcsWorld world, UnityEngine.Component component, int entity)
        {
            var movableView = component.GetComponent<MovingObject>();
            if (movableView == null)
                return;

            var movableViewsPool = world.GetPool<MovableViewComponent>();

            ref var movableViewComponent = ref movableViewsPool.Add(entity);
            movableViewComponent.View = movableView;

            var movementSpeedsToPool = world.GetPool<MovementSpeedToComponent>();

            ref var movementSpeedToComponent = ref movementSpeedsToPool.Add(entity);
            movementSpeedToComponent.Value = movableView.Speed;

            var positionsPool = world.GetPool<PositionComponent>();
            ref var positionComponent = ref positionsPool.Add(entity);
            positionComponent.Value = component.transform.position;
        }

        private static void InitAccelerationToComponent(EcsWorld world, UnityEngine.Component transform, int entity)
        {
            var accelerationView = transform.GetComponent<AccelerationObject>();
            if (accelerationView == null)
                return;

            var accelerationsToPool = world.GetPool<AccelerationToComponent>();

            ref var accelerationToComponent = ref accelerationsToPool.Add(entity);
            accelerationToComponent.Value = accelerationView.MaxDuration;
        }
    }
}