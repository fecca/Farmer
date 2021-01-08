using System.Collections.Generic;
using UnityEngine;

public interface ILevelRepresentation
{
    void DrawLevel(float[,] level, int x, int z);
}

public class MeshLevel : ILevelRepresentation
{
    private Mesh _mesh;

    public MeshLevel()
    {
        var go = new GameObject("Level");
        go.AddComponent<MeshRenderer>();
        _mesh = go.AddComponent<MeshFilter>().mesh;
    }

    public void DrawLevel(float[,] level, int centerX, int centerZ)
    {
        var centerPosition = new Vector2Int(centerX, centerZ);
        var radius = 3;
        var xSize = radius * 2 + 1;
        var zSize = radius * 2 + 1;
        var vertices = new Vector3[xSize * zSize];

        for (int i = 0, x = centerPosition.x - radius; x < centerPosition.x + radius + 1; x++)
        {
            for (var z = centerPosition.y - radius; z < centerPosition.y + radius + 1; z++, i++)
            {
                if (IsInRange(level, x, z))
                {
                    vertices[i] = new Vector3(x, level[x, z], z);
                }
            }
        }

        var triangles = new int[(xSize) * (zSize) * 6];
        for (int triangleIndex = 0, vertexIndex = 0, x = 0; x < xSize - 1; x++, vertexIndex++)
        {
            for (var z = 0; z < zSize - 1; z++, triangleIndex += 6, vertexIndex++)
            {
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = vertexIndex + zSize + 1;

                triangles[triangleIndex + 3] = vertexIndex + zSize + 1;
                triangles[triangleIndex + 4] = vertexIndex + zSize;
                triangles[triangleIndex + 5] = vertexIndex;
            }
        }

        _mesh.SetVertices(vertices);
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }

    private bool IsInRange(float[,] level, int x, int z)
    {
        return x >= 0 && x < level.GetLength(0) && z >= 0 && z < level.GetLength(1);
    }
}

public class CubeLevel : ILevelRepresentation
{
    private readonly NodeBehaviour.Factory _nodeFactory;

    public CubeLevel(NodeBehaviour.Factory nodeFactory)
    {
        _nodeFactory = nodeFactory;
    }

    public void DrawLevel(float[,] level, int centerX, int centerz)
    {
        for (var x = 0; x < level.GetLength(0); x++)
        {
            for (var z = 0; z < level.GetLength(1); z++)
            {
                _nodeFactory.Create(null, x, z, level[x, z], 0);
            }
        }
    }

    // private bool IsInRange(int x, int z)
    // {
    //     return x >= 0 && x < _level.GetLength(0) && z >= 0 && z < _level.GetLength(1);
    // }
}

public class LevelService : ILevelService
{
    private readonly ILevelGenerator _levelGenerator;
    private readonly ILevelRepresentation _levelRepresentation;

    private float[,] _level;

    public LevelService(ILevelGenerator levelGenerator, ILevelRepresentation levelRepresentation)
    {
        _levelRepresentation = levelRepresentation;
        _levelGenerator = levelGenerator;
    }

    public void GenerateLevel()
    {
        _level = _levelGenerator.GenerateLevel();
    }

    public void DrawLevel(int x, int z)
    {
        _levelRepresentation.DrawLevel(_level, x, z);
    }

    public float[,] GetLevel()
    {
        return _level;
    }

    public void CenterNodes(int x, int z)
    {
        // _levelGenerator.CenterNodes(x, z);
    }

    public void SetStartNodes(int x, int z)
    {
        // _levelGenerator.SetStartNodes(x, z);
    }
}