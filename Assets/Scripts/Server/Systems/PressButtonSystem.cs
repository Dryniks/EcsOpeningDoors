using Leopotam.EcsLite;
using EcsOpeningDoors.Component;

namespace Server.Systems
{
    public class PressButtonSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _actorFilter;
        private EcsFilter _buttonFilter;

        private EcsPool<Position> _positionsPool;
        private EcsPool<Button> _buttonsPool;
        private EcsPool<Link> _linksPool;
        private EcsPool<PressButton> _pressButtonsPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _actorFilter = world.Filter<Actor>().End();
            _buttonFilter = world.Filter<Button>().End();

            _positionsPool = world.GetPool<Position>();
            _buttonsPool = world.GetPool<Button>();
            _linksPool = world.GetPool<Link>();
            _pressButtonsPool = world.GetPool<PressButton>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var actorEntity in _actorFilter)
            {
                var actorPosition = _positionsPool.Get(actorEntity).Value;

                foreach (var buttonEntity in _buttonFilter)
                {
                    var radius = _buttonsPool.Get(buttonEntity).Radius;
                    var buttonPosition = _positionsPool.Get(buttonEntity).Value;
                    var buttonLink = _linksPool.Get(buttonEntity).Id;

                    var distance = (actorPosition - buttonPosition).magnitude;
                    if (distance > radius)
                        continue;

                    var world = systems.GetWorld();
                    var entity = world.NewEntity();

                    ref var link = ref _linksPool.Add(entity);
                    link.Id = buttonLink;

                    _pressButtonsPool.Add(entity);
                }
            }
        }
    }
}