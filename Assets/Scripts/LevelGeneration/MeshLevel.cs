using UnityEngine;

public class MeshLevel : ILevelRepresentation
{
    private Mesh _mesh;
    private Mesh _meshSideL;
    private Mesh _meshSideR;
    private Mesh _meshSideB;
    private Mesh _meshSideT;
    private float _worldHeight = 10f;

    public MeshLevel()
    {
        var groundMaterial = Resources.Load<Material>("Leaves");
        var baseMaterial = Resources.Load<Material>("Trunk");

        var go = new GameObject("Level");
        go.AddComponent<MeshRenderer>().material = groundMaterial;
        _mesh = go.AddComponent<MeshFilter>().mesh;

        var goSideL = new GameObject("Level_Side");
        goSideL.AddComponent<MeshRenderer>().material = baseMaterial;
        _meshSideL = goSideL.AddComponent<MeshFilter>().mesh;

        var goSideR = new GameObject("Level_Side");
        goSideR.AddComponent<MeshRenderer>().material = baseMaterial;
        _meshSideR = goSideR.AddComponent<MeshFilter>().mesh;

        var goSideB = new GameObject("Level_Side");
        goSideB.AddComponent<MeshRenderer>().material = baseMaterial;
        _meshSideB = goSideB.AddComponent<MeshFilter>().mesh;

        var goSideT = new GameObject("Level_Side");
        goSideT.AddComponent<MeshRenderer>().material = baseMaterial;
        _meshSideT = goSideT.AddComponent<MeshFilter>().mesh;
    }

    public void DrawLevel(float[,] level, int playerX, int playerZ)
    {
        var radius = 4;
        var xSize = radius * 2 + 1;
        var zSize = radius * 2 + 1;
        var centerX = Mathf.Clamp(playerX, 0 + radius, level.GetLength(0) - radius - 2);
        var centerZ = Mathf.Clamp(playerZ, 0 + radius, level.GetLength(1) - radius - 2);

        var leftMost = centerX - radius;
        var rightMost = leftMost + xSize;
        var bottomMost = centerZ - radius;
        var topMost = bottomMost + zSize;

        var vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, x = leftMost; x <= rightMost; x++)
        {
            for (var z = bottomMost; z <= topMost; z++, i++)
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
        _mesh.RecalculateBounds();

        DrawLeftSide(zSize, bottomMost, topMost, leftMost, level);
        DrawRightSide(zSize, bottomMost, topMost, rightMost, level);
        DrawBottomSide(xSize, leftMost, rightMost, bottomMost, level);
        DrawTopSide(xSize, leftMost, rightMost, topMost, level);
    }

    private void DrawLeftSide(int zSize, int bottomMost, int topMost, int leftMost, float[,] level)
    {
        var sideVertices = new Vector3[(zSize + 1) * 2];
        for (int i = 0, z = bottomMost; z <= topMost; z++, i += 2)
        {
            sideVertices[i] = new Vector3(leftMost, -_worldHeight, z);
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
        _meshSideL.RecalculateBounds();
    }

    private void DrawRightSide(int zSize, int bottomMost, int topMost, int rightMost, float[,] level)
    {
        var sideVertices = new Vector3[(zSize + 1) * 2];
        for (int i = 0, z = bottomMost; z <= topMost; z++, i += 2)
        {
            sideVertices[i] = new Vector3(rightMost, -_worldHeight, z);
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
        _meshSideR.RecalculateBounds();
    }


    private void DrawBottomSide(int xSize, int leftMost, int rightMost, int bottomMost, float[,] level)
    {
        var sideVertices = new Vector3[(xSize + 1) * 2];
        for (int i = 0, x = leftMost; x <= rightMost; x++, i += 2)
        {
            sideVertices[i] = new Vector3(x, -_worldHeight, bottomMost);
            sideVertices[i + 1] = new Vector3(x, level[x, bottomMost], bottomMost);
        }

        var sideTriangles = new int[xSize * 6];
        for (int triangleIndex = 0, vertexIndex = 0, x = 0; x < xSize; x++, triangleIndex += 6, vertexIndex += 2)
        {
            sideTriangles[triangleIndex] = vertexIndex;
            sideTriangles[triangleIndex + 1] = vertexIndex + 1;
            sideTriangles[triangleIndex + 2] = vertexIndex + 2;

            sideTriangles[triangleIndex + 3] = vertexIndex + 2;
            sideTriangles[triangleIndex + 4] = vertexIndex + 1;
            sideTriangles[triangleIndex + 5] = vertexIndex + 3;
        }

        _meshSideB.SetVertices(sideVertices);
        _meshSideB.triangles = sideTriangles;
        _meshSideB.RecalculateNormals();
        _meshSideB.RecalculateBounds();
    }


    private void DrawTopSide(int xSize, int leftMost, int rightMost, int topMost, float[,] level)
    {
        var sideVertices = new Vector3[(xSize + 1) * 2];
        for (int i = 0, x = leftMost; x <= rightMost; x++, i += 2)
        {
            sideVertices[i] = new Vector3(x, -_worldHeight, topMost);
            sideVertices[i + 1] = new Vector3(x, level[x, topMost], topMost);
        }

        var sideTriangles = new int[xSize * 6];
        for (int triangleIndex = 0, vertexIndex = 0, x = 0; x < xSize; x++, triangleIndex += 6, vertexIndex += 2)
        {
            sideTriangles[triangleIndex] = vertexIndex;
            sideTriangles[triangleIndex + 1] = vertexIndex + 2;
            sideTriangles[triangleIndex + 2] = vertexIndex + 1;

            sideTriangles[triangleIndex + 3] = vertexIndex + 1;
            sideTriangles[triangleIndex + 4] = vertexIndex + 2;
            sideTriangles[triangleIndex + 5] = vertexIndex + 3;
        }

        _meshSideT.SetVertices(sideVertices);
        _meshSideT.triangles = sideTriangles;
        _meshSideT.RecalculateNormals();
        _meshSideT.RecalculateBounds();
    }
}