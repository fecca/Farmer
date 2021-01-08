using ARPG.Zenject;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    public TreeBehaviour _treePrefab;
    public int startingZ = 10;
    public int startingX = 10;

    private ILevelService _levelService;
    private PlayerController _playerController;
    private ISignalBusAdapter _signalBusAdapter;

    [Inject]
    private void Construct(ILevelService levelService, PlayerController playerController, ISignalBusAdapter signalBusAdapter)
    {
        _levelService = levelService;
        _playerController = playerController;
        _signalBusAdapter = signalBusAdapter;
    }

    private void Start()
    {
        _signalBusAdapter.Subscribe<EnteredNodeSignal>(OnEnteredNode);

        _levelService.GenerateLevel();

        SpawnPlayer();
        SpawnTree();
    }

    private void OnEnteredNode(EnteredNodeSignal signal)
    {
        _levelService.CenterNodes(signal.x, signal.z);
    }

    private void SpawnPlayer()
    {
        var node = _levelService.GetLevel()[startingX, startingZ];
        var position = new Vector3(startingX, node.Elevation, startingZ);
        _playerController.SetPosition(position);

        _levelService.SetStartNodes(startingX, startingZ);
    }

    private void SpawnTree()
    {
        var tree = Instantiate(_treePrefab);
        var node = _levelService.GetLevel()[2, 16];
        var position = new Vector3(node.Coordinates.x, node.Elevation, node.Coordinates.y);
        tree.transform.position = position;
    }
}