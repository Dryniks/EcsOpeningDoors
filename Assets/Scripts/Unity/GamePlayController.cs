using EcsOpeningDoors.System;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace EcsOpeningDoors.Unity
{
    public class GamePlayController : MonoBehaviour
    {
        private EcsWorld _ecsWorld;
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;

        [Inject]
        private void Construct(IInputController inputController)
        {
            _ecsWorld = new EcsWorld();
            
            _updateSystems = new EcsSystems(_ecsWorld);
            _updateSystems.Add(new InputSystem(inputController));
            
            _updateSystems.Init();
        }

        private void Update()
        {
            _updateSystems?.Run();
        }
    }
}