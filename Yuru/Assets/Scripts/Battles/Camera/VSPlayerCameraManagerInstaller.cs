using System;
using Systems;
using Zenject;

namespace Battles.Camera {
    public class VSPlayerCameraManagerInstaller : MonoInstaller<VSPlayerCameraManagerInstaller> {
        public override void InstallBindings() {
            Container.Bind<CameraManagerBase>().To<VSPlayerCameraManager>().AsCached();
        }
    }
}
