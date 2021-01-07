using Rovio.Templates.Client.Commons.Zenject;
using Zenject;

namespace ARPG.Zenject
{
	public class ZenjectInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);

            Container.Bind<ISignalBusAdapter>().To<SignalBusAdapter>().AsSingle();
            Container.Bind<IDiContainerAdapter>().To<DiContainerAdapter>().AsSingle();
		}
	}
}