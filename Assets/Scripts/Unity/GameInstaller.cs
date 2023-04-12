using UnityEngine;
using Zenject;

namespace EcsOpeningDoors.Unity
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Transform _characterSpawnPoint;
        [SerializeField] private GameObject _characterPrefab;

        public override void InstallBindings()
        {
            BindCharacter();
            BindInputController();
            BindTimeService();
        }

        private void BindInputController()
        {
            Container.BindInterfacesTo<InputController>().AsSingle();
        }

        private void BindTimeService()
        {
            Container.Bind<TimeService>().AsSingle();
        }

        private void BindCharacter()
        {
            var character = Container.InstantiatePrefabForComponent<ActorView>(_characterPrefab,
                _characterSpawnPoint.position, _characterSpawnPoint.rotation, null);

            Container.BindInterfacesTo<ActorView>().FromInstance(character).AsSingle();
        }
    }
}