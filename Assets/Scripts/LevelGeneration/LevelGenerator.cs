using System.Linq;
using UnityEngine;

public class LevelGenerator : ILevelGenerator
{
    private readonly NodeBehaviour.Factory _nodeFactory;

    private NodeBehaviour[,] _level;
    private float _timer;
    private Transform _parent;

    public LevelGenerator(NodeBehaviour.Factory nodeFactory)
    {
        _nodeFactory = nodeFactory;
    }

    public NodeBehaviour[,] GenerateLevel(LevelConfig levelConfig)
    {
        ClearLevel();

        var width = levelConfig.width;
        var height = levelConfig.height;
        var seed = levelConfig.seed;
        var xChunks = levelConfig.xChunks;
        var zChunks = levelConfig.zChunks;

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
                        var elevation = levelConfig.noiseLayers.Sum(t => t.Evaluate(x / levelConfig.globalNoiseScale, z / levelConfig.globalNoiseScale, seed));
                        elevation += levelConfig.worldElevation;
                        var node = _nodeFactory.Create(chunkParent, x, z, elevation, levelConfig.waterLevel);

                        _level[x, z] = node;
                    }
                }
            }
        }


        return _level;
    }

    private void ClearLevel()
    {
        if (_level == null) return;

        Object.Destroy(_parent.gameObject);

        _level = null;
    }
}