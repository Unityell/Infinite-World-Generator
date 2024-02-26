using Zenject;

public class GameInfoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameInfo>().AsSingle();
    }
}