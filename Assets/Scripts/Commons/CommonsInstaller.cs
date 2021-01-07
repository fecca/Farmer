using ARPG.Moving;
using Zenject;

public class CommonsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Raycaster>().AsSingle();
    }
}