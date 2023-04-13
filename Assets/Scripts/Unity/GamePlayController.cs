using EcsOpeningDoors.System;
using EcsOpeningDoors.System.Unity;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace EcsOpeningDoors.Unity
{
    public class GamePlayController : MonoBehaviour
    {
        private TimeService _timeService;

        private EcsWorld _ecsWorld;
        private IEcsSystems _initSystems;
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;

        [Inject]
        private void Construct(IInputController inputController, IActorView actorView, TimeService timeService)
        {
            _timeService = timeService;

            _ecsWorld = new EcsWorld();

            _initSystems = new EcsSystems(_ecsWorld)
                .Add(new CreateLevelSystem());

            _initSystems.Init();

            _updateSystems = new EcsSystems(_ecsWorld)
                .Add(new InputSystem(inputController))
                .Add(new InterruptionSystem())
                .Add(new CheckRotationCompletionSystem())
                .Add(new AccelerationSystem(_timeService))
                .Add(new CheckMovementCompletionSystem())
                .Add(new MovementAnimationSystem())
                .Add(new CleaningSystem());

            _updateSystems.Init();

            _fixedUpdateSystems = new EcsSystems(_ecsWorld)
                .Add(new RotationSystem(timeService))
                .Add(new MovementSystem(timeService));

            _fixedUpdateSystems.Init();
        }

        private void Update()
        {
            if (_timeService != null)
                _timeService.DeltaTime = Time.deltaTime;

            _updateSystems?.Run();
        }

        private void FixedUpdate()
        {
            if (_timeService != null)
                _timeService.FixedDeltaTime = Time.fixedDeltaTime;

            _fixedUpdateSystems?.Run();
        }

        private void OnDestroy()
        {
            _initSystems?.Destroy();
            _updateSystems?.Destroy();
            _fixedUpdateSystems?.Destroy();
            _lateUpdateSystems?.Destroy();
            _ecsWorld?.Destroy();
        }
    }
}