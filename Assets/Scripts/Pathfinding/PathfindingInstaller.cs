using Zenject;

public class PathfindingInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPathfindingService>().To<PathfindingService>().AsSingle();
    }
}