using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    private ILevelService _levelService;
    private PlayerController _playerController;

    [Inject]
    private void Construct(ILevelService levelService, PlayerController playerController)
    {
        _playerController = playerController;
        _levelService = levelService;
    }

    private void Start()
    {
        _levelService.GenerateLevel();

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var startingNode = _levelService.GetLevel()[0, 0];
        var playerPosition = new Vector3(startingNode.Coordinates.x, startingNode.Elevation, startingNode.Coordinates.y);
        _playerController.SetPosition(playerPosition);
    }
}