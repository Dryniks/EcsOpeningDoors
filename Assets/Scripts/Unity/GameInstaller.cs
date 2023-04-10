using Zenject;

namespace EcsOpeningDoors.Unity
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInputController();
        }

        private void BindInputController()
        {
            Container.BindInterfacesTo<InputController>().AsSingle();
        }
    }
}