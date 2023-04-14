using System;
using System.Collections.Generic;
using EcsOpeningDoors.Component;
using EcsOpeningDoors.System;
using EcsOpeningDoors.System.Unity;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace EcsOpeningDoors.Unity
{
    public class Startup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _ecsSystems;
        private Server _server;
        
        private EcsPool<ClickPositionComponent> _clickPositionsPool;

        [Inject]
        private void Construct(IEnumerable<IEcsSystem> systems)
        {
            _world = new EcsWorld();
            
            _ecsSystems = new EcsSystems(_world);
            _ecsSystems.Add(new CreateLevelSystem());
            _ecsSystems.Init();
            
            _clickPositionsPool = _world.GetPool<ClickPositionComponent>();

            _server = new Server(_world, systems);
            _server.Run();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            var cam = Camera.main;
            if (cam == null)
                return;

            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity))
                return;

            Vector3? targetPosition = hit.transform.CompareTag(TagManager.Ground) ? hit.point : null;
            if (!targetPosition.HasValue)
                return;

            var entity = _world.NewEntity();
            Debug.LogError(entity);
            
            ref var clickPosition = ref _clickPositionsPool.Add(entity);
            clickPosition.Value = targetPosition.Value;
        }

        private void OnDestroy()
        {
            _ecsSystems?.Destroy();
            _ecsSystems = null;
            
            _server.Dispose();
            
            _world?.Destroy();
            _world = null;
        }
    }
}