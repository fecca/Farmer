using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ARPG.Moving;
using ARPG.Zenject;
using Farmer;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public AnimationCurve _jumpCurve;

    private bool _jumping;
    private float _timer;
    private float _time = 0.1f;
    private Raycaster _raycaster;
    private ISignalBusAdapter _signalBusAdapter;
    private List<Vector2Int> _path = new List<Vector2Int>();
    private ILevelService _levelService;
    private IPathfindingService _pathfindingService;

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
        _signalBusAdapter.Subscribe<MouseButtonDownSignal>(OnMouseButtonDown);
    }

    private void OnMouseButtonDown(MouseButtonDownSignal signal)
    {
        if (signal.button != 0) return;

        if (!_raycaster.RaycastGround(Input.mousePosition, out var hit)) return;
        var node = hit.collider.GetComponent<NodeBehaviour>();
        if (node == null) return;

        CalculatePath(node);
    }

    private void CalculatePath(NodeBehaviour targetNodeBehaviour)
    {
        var start = new Vector2Int((int) transform.position.x, (int) transform.position.z);
        var end = new Vector2Int((int) targetNodeBehaviour.transform.position.x, (int) targetNodeBehaviour.transform.position.z);

        _path = _pathfindingService.FindPath(_levelService.GetLevel(), start, end);

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        foreach (var pathNode in _path.Skip(1))
        {
            var from = new Vector3(pathNode.x, 0, pathNode.y) + Vector3.up * 10f;
            var to = new Vector3(pathNode.x, 0, pathNode.y) + Vector3.down * 10f;
            if (!_raycaster.Raycast(from, to, out var hit)) continue;

            var newPosition = hit.point + Vector3.up * 0.5f;
            yield return Hop(newPosition, 0.25f);

            transform.position = newPosition;
        }
    }

    IEnumerator Hop(Vector3 dest, float time)
    {
        var startPos = transform.position;
        var timer = 0.0f;

        while (timer <= 1.0f)
        {
            var height = Mathf.Sin(Mathf.PI * timer) * 1.5f;
            transform.position = Vector3.Lerp(startPos, dest, timer) + Vector3.up * height;

            timer += Time.deltaTime / time;
            yield return null;
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}