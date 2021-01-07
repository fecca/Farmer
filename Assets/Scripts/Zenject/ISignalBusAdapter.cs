using System;

namespace ARPG.Zenject
{
	public interface ISignalBusAdapter
	{
		void Subscribe<TSignal>(Action<TSignal> callback);
		void Fire<TSignal>(TSignal signal);
	}
}