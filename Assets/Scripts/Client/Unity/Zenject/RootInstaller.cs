using Zenject;

namespace EcsOpeningDoors.Unity
{
    public class RootInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<TimeService>().AsSingle();
            Container.BindInterfacesTo<Client>().AsSingle();
            Container.Bind<World>().AsSingle();
            Container.Bind<Server>().AsSingle();
        }
    }
}