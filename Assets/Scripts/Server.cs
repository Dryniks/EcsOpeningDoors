using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EcsOpeningDoors.Component;
using Leopotam.EcsLite;
using Debug = UnityEngine.Debug;

namespace EcsOpeningDoors
{
    public class Server : IDisposable
    {
        private readonly TimeService _timeService = new();
        
        private EcsSystems _systems;
  private EcsFilter _interruptionFilter;
        private CancellationTokenSource _cts;
        private Thread _thread;

        public Server(EcsWorld world ,IEnumerable<IEcsSystem> systems)
        {
            _systems = new EcsSystems(world, _timeService);
            _interruptionFilter = world.Filter<ClickPositionComponent>().End();

            // foreach (var system in systems)
            //     _systems.Add(system);

            _systems.Init();
        }

        public void Run()
        {
            _cts = new CancellationTokenSource();

            _thread = new Thread(Update);
            _thread.Start();
        }

        private void Update()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (!_cts.IsCancellationRequested)
            {
                Debug.LogError(_interruptionFilter.GetEntitiesCount());
                _systems?.Run();

                _timeService.DeltaTime = stopWatch.ElapsedTicks / 10000.0f;

                stopWatch.Restart();
            }
            
            stopWatch.Stop();
        }

        public void Dispose()
        {
            _thread.Abort();
            _thread = null;
            
            _cts?.Dispose();
            _cts = null;
            
            _systems?.Destroy();
            _systems = null;
        }
    }
}