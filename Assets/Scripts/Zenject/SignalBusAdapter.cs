using System;
using UnityEngine;
using Zenject;

namespace ARPG.Zenject
{
	public class SignalBusAdapter : ISignalBusAdapter
	{
		private readonly SignalBus _signalBus;

		public SignalBusAdapter(SignalBus signalBus)
		{
			_signalBus = signalBus;
		}

		public void Subscribe<TSignal>(Action<TSignal> callback)
		{
			_signalBus.Subscribe(callback);
		}

		public void Fire<TSignal>(TSignal signal)
		{
			Debug.Log($"Firing signal of type {typeof(TSignal)}");
			_signalBus.Fire(signal);
		}
	}
}