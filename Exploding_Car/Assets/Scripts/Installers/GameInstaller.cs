using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);


        Container.DeclareSignal<OnSpawnerRateTimeChangeSignal>();

        Container.Bind(typeof(ISpawner<NPCType>)).WithId("NPCSpawner").To(typeof(NPCSpawnerController)).AsSingle();
        Container.Bind(typeof(ISpawner<ParticleType>)).WithId("ParticleSpawner").To(typeof(ParticleSpawnerController)).AsSingle();
    }
}
