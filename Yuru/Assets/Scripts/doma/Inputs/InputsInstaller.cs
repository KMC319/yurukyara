using Zenject;

namespace doma.Inputs{
    public class InputsInstaller : MonoInstaller<InputsInstaller>
    {
        public override void InstallBindings(){
            Container
                .Bind<InputRelayPoint>()  
                .To<InputRelayPoint>() 
                .AsCached();    
        }
    }
}