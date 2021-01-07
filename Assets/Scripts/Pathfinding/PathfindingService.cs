using System.Collections.Generic;
using UnityEngine;

public class PathfindingService : IPathfindingService
{
    public List<Vector2Int> FindPath(NodeBehaviour[,] level, Vector2Int start, Vector2Int end)
    {
        return new Astar(Astar.ConvertToBoolArray(level), start, end, Astar.Type.Diagonal).Result;
    }
}