using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EcsOpeningDoors
{
    public class Server : IDisposable
    {
        private const int MillisecondsPerSecond = 1000;

        private readonly IClient _client;
        private readonly World _world;
        private readonly TimeService _timeService;
        private readonly int _frequency;

        private readonly CancellationTokenSource _cts = new();

        public Server(IClient client, TimeService timeService, World world, int fps = 60)
        {
            _client = client;
            _frequency = MillisecondsPerSecond / fps;

            _world = world;
            _timeService = timeService;
        }

        public void Init(Dictionary<int, List<object>> dictionary)
        {
            var data = _world.GetStartData(dictionary);
            _client.UpdateData(data);

            Run();
        }

        private async void Run()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                while (!_cts.IsCancellationRequested)
                {
                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    stopwatch.Restart();

                    _timeService.DeltaTime = elapsedMilliseconds / (float) MillisecondsPerSecond;
                    Update();

                    var delay = _frequency - stopwatch.ElapsedMilliseconds;

                    if (delay > 0)
                        await Task.Delay(TimeSpan.FromMilliseconds(delay));
                    else
                        await Task.Yield();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Update()
        {
            if (_world == null)
                return;

            _world.Update();
            _client.UpdateData(_world.GetData());
        }

        public void AppendData(Type type, object data) => _world.AppendData(type, data);

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _world.Dispose();
        }
    }
}