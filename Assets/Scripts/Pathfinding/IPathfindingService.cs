using System.Collections.Generic;
using UnityEngine;

public interface IPathfindingService
{
    List<Vector2Int> FindPath(NodeBehaviour[,] level, Vector2Int start, Vector2Int end);
}