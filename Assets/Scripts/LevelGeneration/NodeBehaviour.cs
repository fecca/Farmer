using ARPG.Zenject;
using UnityEngine;
using Zenject;

public class NodeBehaviour : MonoBehaviour, IInteractable
{
    private Material _material;
    private ISignalBusAdapter _signalBusAdapter;
    private Transform _parent;
    private float _waterLevel;
    private bool _highlighted;
    private int _x;
    private int _z;

    public bool IsWalkable => transform.position.y > _waterLevel;
    public Vector2Int Coordinates => new Vector2Int(_x, _z);
    public float Elevation { get; private set; }

    [Inject]
    private void Construct(ISignalBusAdapter signalBusAdapter, Transform parent, int x, int z, float elevation, float waterLevel)
    {
        _signalBusAdapter = signalBusAdapter;
        _parent = parent;
        _x = x;
        _z = z;
        Elevation = elevation;
        _waterLevel = waterLevel;
    }

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;

        var tf = transform;
        tf.SetParent(_parent);
        tf.position = new Vector3(_x, Elevation / 2f, _z);
        tf.localScale = new Vector3(1, Elevation, 1);
    }

    private void Update()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (_highlighted)
        {
            _material.color = Color.red;
        }
        else if (IsWalkable)
        {
            _material.color = Color.green;
        }
        else
        {
            _material.color = Color.blue;
        }
    }

    public void Hover(bool isHovering)
    {
        _highlighted = isHovering;
    }

    public void Interact()
    {
        _signalBusAdapter.Fire(new NodeBehaviourClickedSignal { nodeBehaviour = this });
    }

    public class Factory : PlaceholderFactory<Transform, int, int, float, float, NodeBehaviour>
    {
    }
}

public class NodeBehaviourClickedSignal
{
    public NodeBehaviour nodeBehaviour;
}