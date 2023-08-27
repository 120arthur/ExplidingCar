using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);


        Container.DeclareSignal<OnSpawnerRateTimeChangeSignal>();

        Container.Bind<SpawnerNPCController>().AsSingle();
        Container.Bind<SpawnParticleController>().AsSingle();
    }
}
