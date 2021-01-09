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