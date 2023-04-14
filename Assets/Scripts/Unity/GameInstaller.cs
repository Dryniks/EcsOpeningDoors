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

            BindSystemInstaller();
        }

        private void BindCharacter()
        {
            var character = Container.InstantiatePrefabForComponent<ActorView>(_characterPrefab,
                _characterSpawnPoint.position, _characterSpawnPoint.rotation, null);

            Container.BindInterfacesTo<ActorView>().FromInstance(character).AsSingle();
        }
        
        private void BindSystemInstaller()
        {
            Container.Install<SystemsInstaller>();
        }
    }
}