using ARPG.Zenject;
using UnityEngine;
using Zenject;

public class InputHandler : MonoBehaviour
{
    private ISignalBusAdapter _signalBusAdapter;

    [Inject]
    private void Construct(ISignalBusAdapter signalBusAdapter)
    {
        _signalBusAdapter = signalBusAdapter;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _signalBusAdapter.Fire(new MouseButtonDownSignal { button = 0 });
        }

        if (Input.GetMouseButtonDown(1))
        {
            _signalBusAdapter.Fire(new MouseButtonDownSignal { button = 1 });
        }
    }
}