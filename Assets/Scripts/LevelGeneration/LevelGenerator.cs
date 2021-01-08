using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : ILevelGenerator
{
    private readonly NodeBehaviour.Factory _nodeFactory;

    private NodeBehaviour[,] _level;
    private float _timer;
    private Transform _parent;
    private readonly LevelConfig _levelConfig;
    private int _radius = 8;
    private readonly List<NodeBehaviour> _currentNodes = new List<NodeBehaviour>();

    public LevelGenerator(NodeBehaviour.Factory nodeFactory, LevelConfig levelConfig)
    {
        _nodeFactory = nodeFactory;
        _levelConfig = levelConfig;
    }

    public NodeBehaviour[,] GenerateLevel()
    {
        ClearLevel();

        var width = _levelConfig.width;
        var height = _levelConfig.height;
        var seed = _levelConfig.seed;
        var xChunks = _levelConfig.xChunks;
        var zChunks = _levelConfig.zChunks;

        _level = new NodeBehaviour[width * xChunks, height * zChunks];
        _parent = new GameObject("Level").transform;

        for (var xChunkIndex = 0; xChunkIndex < xChunks; xChunkIndex++)
        {
            for (var zChunkIndex = 0; zChunkIndex < zChunks; zChunkIndex++)
            {
                var chunkParent = new GameObject($"Chunk({xChunkIndex}, {zChunkIndex})").transform;
                chunkParent.SetParent(_parent);

                for (var x = width * xChunkIndex; x < width * (xChunkIndex + 1); x++)
                {
                    for (var z = height * zChunkIndex; z < height * (zChunkIndex + 1); z++)
                    {
                        var elevation = _levelConfig.noiseLayers.Sum(
                            t => t.Evaluate(x / _levelConfig.globalNoiseScale, z / _levelConfig.globalNoiseScale, seed));
                        elevation += _levelConfig.worldElevation;
                        var node = _nodeFactory.Create(chunkParent, x, z, elevation, _levelConfig.waterLevel);

                        _level[x, z] = node;
                    }
                }
            }
        }


        return _level;
    }

    public void SetStartNodes(int xCoordinate, int zCoordinate)
    {
        for (var x = xCoordinate - _radius; x <= xCoordinate + _radius; x++)
        {
            for (var z = zCoordinate - _radius; z <= zCoordinate + _radius; z++)
            {
                if (x < 0 || x >= _levelConfig.width) continue;
                if (z < 0 || z >= _levelConfig.height) continue;

                var nodeBehaviour = _level[x, z];
                _currentNodes.Add(nodeBehaviour);

                nodeBehaviour.Toggle(true);
            }
        }
    }

    public void CenterNodes(int xCoordinate, int zCoordinate)
    {
        var nodes = new List<NodeBehaviour>();

        for (var x = xCoordinate - _radius; x <= xCoordinate + _radius; x++)
        {
            for (var z = zCoordinate - _radius; z <= zCoordinate + _radius; z++)
            {
                if (IsInRange(x, z))
                {
                    nodes.Add(_level[x, z]);
                }
            }
        }

        var oldNodes = _currentNodes.Where(cn => !nodes.Contains(cn)).ToList();
        var newNodes = nodes.Where(cn => !_currentNodes.Contains(cn)).ToList();

        foreach (var node in oldNodes)
        {
            node.Toggle(false);
            _currentNodes.Remove(node);
        }

        foreach (var node in newNodes)
        {
            node.Toggle(true);
            _currentNodes.Add(node);
        }
    }

    private bool IsInRange(int x, int z)
    {
        return x >= 0 && x < _levelConfig.width && z >= 0 && z < _levelConfig.height;
    }

    private void ClearLevel()
    {
        if (_level == null) return;

        Object.Destroy(_parent.gameObject);

        _level = null;
    }
}