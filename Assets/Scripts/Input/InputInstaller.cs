using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<MouseButtonDownSignal>();
    }
}