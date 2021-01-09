using UnityEngine;

public interface ILevelRepresentation
{
    void DrawLevel(float[,] level, int playerX, int playerZ);
}

public class MeshLevel : ILevelRepresentation
{
    private Mesh _mesh;
    private Mesh _meshSideL;
    private Mesh _meshSideR;
    private Mesh _meshSideB;
    private Mesh _meshSideT;

    public MeshLevel()
    {
        var go = new GameObject("Level");
        go.AddComponent<MeshRenderer>();
        _mesh = go.AddComponent<MeshFilter>().mesh;

        var goSideL = new GameObject("Level_Side");
        goSideL.AddComponent<MeshRenderer>();
        _meshSideL = goSideL.AddComponent<MeshFilter>().mesh;

        var goSideR = new GameObject("Level_Side");
        goSideR.AddComponent<MeshRenderer>();
        _meshSideR = goSideR.AddComponent<MeshFilter>().mesh;

        var goSideB = new GameObject("Level_Side");
        goSideB.AddComponent<MeshRenderer>();
        _meshSideB = goSideB.AddComponent<MeshFilter>().mesh;

        var goSideT = new GameObject("Level_Side");
        goSideT.AddComponent<MeshRenderer>();
        _meshSideT = goSideT.AddComponent<MeshFilter>().mesh;
    }

    public void DrawLevel(float[,] level, int playerX, int playerZ)
    {
        var radius = 1;
        var xSize = radius * 2 + 1;
        var zSize = radius * 2 + 1;
        var centerX = Mathf.Clamp(playerX, 0 + radius, level.GetLength(0) - radius - 2);
        var centerZ = Mathf.Clamp(playerZ, 0 + radius, level.GetLength(1) - radius - 2);

        var leftMost = centerX - radius;
        var rightMost = leftMost + xSize + 1;
        var bottomMost = centerZ - radius;
        var topMost = bottomMost + zSize + 1;

        var vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, x = leftMost; x < rightMost; x++)
        {
            for (var z = bottomMost; z < topMost; z++, i++)
            {
                vertices[i] = new Vector3(x, level[x, z], z);
            }
        }

        var triangles = new int[xSize * zSize * 6];
        for (int triangleIndex = 0, vertexIndex = 0, x = 0; x < xSize; x++, vertexIndex++)
        {
            for (var z = 0; z < zSize; z++, triangleIndex += 6, vertexIndex++)
            {
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 1;
                triangles[triangleIndex + 2] = vertexIndex + zSize + 2;

                triangles[triangleIndex + 3] = vertexIndex + zSize + 2;
                triangles[triangleIndex + 4] = vertexIndex + zSize + 1;
                triangles[triangleIndex + 5] = vertexIndex;
            }
        }

        _mesh.SetVertices(vertices);
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();

        DrawLeftSide(zSize, bottomMost, topMost, leftMost, level);
        DrawRightSide(zSize, bottomMost, topMost, rightMost, level);
    }

    private void DrawLeftSide(int zSize, int bottomMost, int topMost, int leftMost, float[,] level)
    {
        var sideVertices = new Vector3[(zSize + 1) * 2];
        for (int i = 0, z = bottomMost; z < topMost; z++, i += 2)
        {
            sideVertices[i] = new Vector3(leftMost, -5f, z);
            sideVertices[i + 1] = new Vector3(leftMost, level[leftMost, z], z);
        }

        var sideTriangles = new int[zSize * 6];
        for (int triangleIndex = 0, vertexIndex = 0, z = 0; z < zSize; z++, triangleIndex += 6, vertexIndex += 2)
        {
            sideTriangles[triangleIndex] = vertexIndex;
            sideTriangles[triangleIndex + 1] = vertexIndex + 2;
            sideTriangles[triangleIndex + 2] = vertexIndex + 1;

            sideTriangles[triangleIndex + 3] = vertexIndex + 1;
            sideTriangles[triangleIndex + 4] = vertexIndex + 2;
            sideTriangles[triangleIndex + 5] = vertexIndex + 3;
        }

        _meshSideL.SetVertices(sideVertices);
        _meshSideL.triangles = sideTriangles;
        _meshSideL.RecalculateNormals();
    }

    private void DrawRightSide(int zSize, int bottomMost, int topMost, int rightMost, float[,] level)
    {
        var sideVertices = new Vector3[(zSize + 1) * 2];
        for (int i = 0, z = bottomMost; z < topMost; z++, i += 2)
        {
            sideVertices[i] = new Vector3(rightMost, -5f, z);
            sideVertices[i + 1] = new Vector3(rightMost, level[rightMost, z], z);
        }

        var sideTriangles = new int[zSize * 6];
        for (int triangleIndex = 0, vertexIndex = 0, z = 0; z < zSize; z++, triangleIndex += 6, vertexIndex += 2)
        {
            sideTriangles[triangleIndex] = vertexIndex;
            sideTriangles[triangleIndex + 1] = vertexIndex + 1;
            sideTriangles[triangleIndex + 2] = vertexIndex + 2;

            sideTriangles[triangleIndex + 3] = vertexIndex + 2;
            sideTriangles[triangleIndex + 4] = vertexIndex + 1;
            sideTriangles[triangleIndex + 5] = vertexIndex + 3;
        }

        _meshSideR.SetVertices(sideVertices);
        _meshSideR.triangles = sideTriangles;
        _meshSideR.RecalculateNormals();
    }
}

public class CubeLevel : ILevelRepresentation
{
    private readonly NodeBehaviour.Factory _nodeFactory;

    public CubeLevel(NodeBehaviour.Factory nodeFactory)
    {
        _nodeFactory = nodeFactory;
    }

    public void DrawLevel(float[,] level, int playerX, int playerZ)
    {
        for (var x = 0; x < level.GetLength(0); x++)
        {
            for (var z = 0; z < level.GetLength(1); z++)
            {
                _nodeFactory.Create(null, x, z, level[x, z], 0);
            }
        }
    }
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