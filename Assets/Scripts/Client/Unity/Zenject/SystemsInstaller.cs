using Leopotam.EcsLite;
using Zenject;
using EcsOpeningDoors.System;
using EcsOpeningDoors.System.Unity;

namespace EcsOpeningDoors.Unity
{
    public class SystemsInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IEcsSystem>().To<InterruptionSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<InputProcessingSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<RotationSystem>().AsTransient();
            Container.Bind<IEcsSystem>().To<AccelerationSystem>().AsSingle();
            Container.Bind<IEcsSystem>().To<MovementSystem>().AsSingle();
        }
    }
}