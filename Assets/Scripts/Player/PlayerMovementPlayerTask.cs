using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ARPG.Moving;
using UnityEngine;

public class PlayerMovementPlayerTask : PlayerTask
{
    private readonly List<Vector2Int> _path;
    private readonly Transform _transform;
    private readonly Raycaster _raycaster;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isRunning;

    public PlayerMovementPlayerTask(List<Vector2Int> path, Transform transform, Raycaster raycaster)
    {
        _path = path;
        _transform = transform;
        _raycaster = raycaster;
    }

    public override PlayerTaskType Type => PlayerTaskType.Move;

    public override async Task Execute()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        await MoveAlongPath();
    }

    public override async Task Cancel()
    {
        _cancellationTokenSource.Cancel();
        while (_isRunning)
        {
            await Task.Yield();
        }
    }

    private async Task MoveAlongPath()
    {
        _isRunning = true;
        foreach (var pathNode in _path.Skip(1))
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _isRunning = false;
                break;
            }

            var from = new Vector3(pathNode.x, 0, pathNode.y) + Vector3.up * 10f;
            var to = new Vector3(pathNode.x, 0, pathNode.y) + Vector3.down * 10f;
            if (!_raycaster.Raycast(from, to, out var hit)) continue;

            var newPosition = hit.point + Vector3.up * 0.5f;
            await Hop(newPosition, 0.25f);

            _transform.position = newPosition;
        }

        _isRunning = false;
    }

    private async Task Hop(Vector3 dest, float time)
    {
        var startPos = _transform.position;
        var timer = 0.0f;

        while (timer <= 1.0f)
        {
            var height = Mathf.Sin(Mathf.PI * timer) * 1.5f;
            _transform.position = Vector3.Lerp(startPos, dest, timer) + Vector3.up * height;

            timer += Time.deltaTime / time;

            await Task.Yield();
        }
    }
}