﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ARPG.Moving;
using ARPG.Zenject;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private Raycaster _raycaster;
    private ISignalBusAdapter _signalBusAdapter;
    private ILevelService _levelService;
    private IPathfindingService _pathfindingService;
    private PlayerTask _playerTask;

    [Inject]
    private void Construct(Raycaster raycaster, ISignalBusAdapter signalBusAdapter, ILevelService levelService, IPathfindingService pathfindingService)
    {
        _raycaster = raycaster;
        _signalBusAdapter = signalBusAdapter;
        _levelService = levelService;
        _pathfindingService = pathfindingService;
    }

    private void Start()
    {
        _signalBusAdapter.Subscribe<NodeBehaviourClickedSignal>(OnNodeBehaviourClicked);
    }

    private float _movementSpeed = 10f;

    private void Update()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
            transform.position + Vector3.up * 15f + Vector3.right * -15f + Vector3.forward * -15f, Time.deltaTime * 5f);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * _movementSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * _movementSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * _movementSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * _movementSpeed;
        }
    }

    private async void OnNodeBehaviourClicked(NodeBehaviourClickedSignal signal)
    {
        await CancelCurrentTask();

        var path = CalculatePath(signal.nodeBehaviour);
        if (path.Count <= 0) return;

        _playerTask = new PlayerMovementPlayerTask(path, transform, _raycaster, _signalBusAdapter);

        await _playerTask.Execute();
    }

    private async Task CancelCurrentTask()
    {
        if (_playerTask != null)
        {
            await _playerTask.Cancel();
            _playerTask = null;
        }
    }

    private List<Vector2Int> CalculatePath(NodeBehaviour targetNodeBehaviour)
    {
        var start = new Vector2Int((int) transform.position.x, (int) transform.position.z);
        var end = new Vector2Int(targetNodeBehaviour.Coordinates.x, targetNodeBehaviour.Coordinates.y);

        return _pathfindingService.FindPath(_levelService.GetLevel(), start, end);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}