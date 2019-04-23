using Zenject;

namespace Battles.Camera {
    public class SoloCameraManagerInstaller : MonoInstaller<SoloCameraManagerInstaller> {
        public override void InstallBindings() {
            Container.Bind<CameraManagerBase>().To<SoloCameraManager>().AsCached();
        }
    }
}
