using EcsOpeningDoors.Component;
using Leopotam.EcsLite;

namespace EcsOpeningDoors.System
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly IInputController _inputController;

        private EcsWorld _world;

        public InputSystem(IInputController inputController)
        {
            _inputController = inputController;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            var pos = _inputController.GetPosition();
            if (!pos.HasValue)
                return;

            var entity = _world.NewEntity();
            var pool = _world.GetPool<MovementRequest>();

            ref var request = ref pool.Add(entity);
            request.Position = pos.Value;
        }
    }
}