using System;
using UnityEngine;
using Zenject;

namespace Farmer
{
    public class NodeBehaviour : MonoBehaviour
    {
        private Material _material;
        private Transform _parent;
        private float _waterLevel;
        private bool _highlighted;
        private int _x;
        private int _z;

        public bool IsWalkable => transform.position.y > _waterLevel;
        public Vector2Int Coordinates => new Vector2Int(_x, _z);
        public float Elevation { get; private set; }

        [Inject]
        private void Construct(Transform parent, int x, int z, float elevation, float waterLevel)
        {
            _parent = parent;
            _x = x;
            _z = z;
            Elevation = elevation;
            _waterLevel = waterLevel;
        }

        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
            transform.SetParent(_parent);
            transform.position = new Vector3(_x, Elevation / 2f, _z);
            transform.localScale = new Vector3(1, Elevation, 1);
        }

        private void Update()
        {
            ResetColor();
        }

        public void ResetColor()
        {
            if (_highlighted)
            {
                _material.color = Color.yellow;
            }
            else if (IsWalkable)
            {
                _material.color = Color.white;
            }
            else
            {
                _material.color = Color.cyan;
            }
        }

        public void Highlight(bool value)
        {
            _highlighted = value;
        }

        public class Factory : PlaceholderFactory<Transform, int, int, float, float, NodeBehaviour>
        {
        }
    }
}