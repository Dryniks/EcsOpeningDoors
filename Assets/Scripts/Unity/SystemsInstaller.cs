using EcsOpeningDoors.System;
using EcsOpeningDoors.System.Unity;
using Leopotam.EcsLite;
using Zenject;

namespace EcsOpeningDoors.Unity
{
    public class SystemsInstaller : Installer
    {
        public override void InstallBindings()
        {
            BindCommonSystems();
        }
        
        private void BindCommonSystems()
        {
            Container.Bind<IEcsSystem>().To<InputProcessingSystem>().AsTransient();
            // Container.Bind<IEcsSystem>().To<InterruptionSystem>().AsTransient();
            // Container.Bind<IEcsSystem>().To<RotationSystem>().AsTransient();
            // Container.Bind<IEcsSystem>().To<CheckRotationCompletionSystem>().AsTransient();
            // Container.Bind<IEcsSystem>().To<AccelerationSystem>().AsTransient();
            // Container.Bind<IEcsSystem>().To<MovementSystem>().AsTransient();
            // Container.Bind<IEcsSystem>().To<CheckMovementCompletionSystem>().AsTransient();
            // Container.Bind<IEcsSystem>().To<MovementAnimationSystem>().AsTransient();
            // Container.Bind<IEcsSystem>().To<CleaningSystem>().AsTransient();
        }
    }
}