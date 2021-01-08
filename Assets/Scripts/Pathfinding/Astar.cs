using System;
using System.Collections.Generic;
using UnityEngine;

public class Astar
{
    public List<Vector2Int> Result { private set; get; }

    private delegate Node[] Find(bool bN, bool bS, bool bE, bool bW, int N, int S, int E, int W, bool[,] grid, int rows, int cols, Node[] result, int i);

    private delegate double Distance(Node start, Node end);

    private readonly Find m_Find;

    public enum Type
    {
        Manhattan,
        Diagonal,
        DiagonalFree,
        Euclidean,
        EuclideanFree
    }

    private class Node
    {
        public int x;
        public int y;
        public Node p;
        public double g;
        public double f;
        public int v;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private Node[] DiagonalSuccessors(bool bN, bool bS, bool bE, bool bW, int N, int S, int E, int W, bool[,] grid, int rows, int cols, Node[] result,
        int i)
    {
        if (bN)
        {
            if (bE && grid[N, E])
            {
                result[i++] = new Node(E, N);
            }

            if (bW && grid[N, W])
            {
                result[i++] = new Node(W, N);
            }
        }

        if (bS)
        {
            if (bE && grid[S, E])
            {
                result[i++] = new Node(E, S);
            }

            if (bW && grid[S, W])
            {
                result[i++] = new Node(W, S);
            }
        }

        return result;
    }

    private Node[] DiagonalSuccessorsFree(bool bN, bool bS, bool bE, bool bW, int N, int S, int E, int W, bool[,] grid, int rows, int cols, Node[] result,
        int i)
    {
        bN = N > -1;
        bS = S < rows;
        bE = E < cols;
        bW = W > -1;

        if (bE)
        {
            if (bN && grid[N, E])
            {
                result[i++] = new Node(E, N);
            }

            if (bS && grid[S, E])
            {
                result[i++] = new Node(E, S);
            }
        }

        if (bW)
        {
            if (bN && grid[N, W])
            {
                result[i++] = new Node(W, N);
            }

            if (bS && grid[S, W])
            {
                result[i++] = new Node(W, S);
            }
        }

        return result;
    }

    private Node[] NothingToDo(bool bN, bool bS, bool bE, bool bW, int N, int S, int E, int W, bool[,] grid, int rows, int cols, Node[] result, int i)
    {
        return result;
    }

    private Node[] Successors(int x, int z, bool[,] grid, int rows, int cols)
    {
        var northNode = z - 1;
        var southNode = z + 1;
        var eastNode = x + 1;
        var westNode = x - 1;

        var isNorthNodeWalkable = northNode > -1 && grid[x, northNode];
        var isSouthNodeWalkable = southNode < rows && grid[x, southNode];
        var isEastNodeWalkable = eastNode < cols && grid[eastNode, z];
        var isWestNodeWalkable = westNode > -1 && grid[westNode, z];

        var result = new Node[8];
        var i = 0;

        if (isNorthNodeWalkable)
        {
            result[i++] = new Node(x, northNode);
        }

        if (isEastNodeWalkable)
        {
            result[i++] = new Node(eastNode, z);
        }

        if (isSouthNodeWalkable)
        {
            result[i++] = new Node(x, southNode);
        }

        if (isWestNodeWalkable)
        {
            result[i++] = new Node(westNode, z);
        }

        return m_Find(isNorthNodeWalkable, isSouthNodeWalkable, isEastNodeWalkable, isWestNodeWalkable, northNode, southNode, eastNode, westNode, grid,
            rows, cols, result, i);
    }

    private double Diagonal(Node start, Node end)
    {
        return Math.Max(Math.Abs(start.x - end.x), Math.Abs(start.y - end.y));
    }

    private double Euclidean(Node start, Node end)
    {
        var x = start.x - end.x;
        var y = start.y - end.y;

        return Math.Sqrt(x * x + y * y);
    }

    private double Manhattan(Node start, Node end)
    {
        return Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
    }

    public static bool[,] ConvertToBoolArray(float[,] grid)
    {
        var arr = new bool[grid.GetLength(0), grid.GetLength(1)];

        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var z = 0; z < grid.GetLength(1); z++)
            {
                arr[x, z] = true; //grid[x, z].IsWalkable;
            }
        }

        return arr;
    }

    public Astar(bool[,] grid, Vector2Int s, Vector2Int e, Type type = Type.Manhattan)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int limit = cols * rows;

        Result = new List<Vector2Int>();

        Dictionary<int, int> list = new Dictionary<int, int>();
        List<Node> open = new List<Node>(new Node[limit]);

        Node node = new Node(s.x, s.y)
        {
            f = 0,
            g = 0,
            v = s.x + s.y * cols
        };

        open.Insert(0, node);

        int length = 1;
        Node adj;

        int i;
        int j;
        double max;
        int min;

        Node current;
        Node[] next;
        Distance distance;

        Node end = new Node(e.x, e.y)
        {
            v = e.x + e.y * cols
        };

        if (type == Type.Diagonal)
        {
            m_Find = DiagonalSuccessors;
            distance = Diagonal;
        }
        else if (type == Type.DiagonalFree)
        {
            m_Find = DiagonalSuccessorsFree;
            distance = Diagonal;
        }
        else if (type == Type.Euclidean)
        {
            m_Find = DiagonalSuccessors;
            distance = Euclidean;
        }
        else if (type == Type.EuclideanFree)
        {
            m_Find = DiagonalSuccessorsFree;
            distance = Euclidean;
        }
        else
        {
            m_Find = NothingToDo;
            distance = Manhattan;
        }

        do
        {
            max = limit;
            min = 0;

            for (i = 0; i < length; i++)
            {
                double f = open[i].f;

                if (f < max)
                {
                    max = f;
                    min = i;
                }
            }

            current = open[min];
            open.RemoveRange(min, 1);

            if (current.v != end.v)
            {
                --length;
                next = Successors(current.x, current.y, grid, rows, cols);

                for (i = 0, j = next.Length; i < j; ++i)
                {
                    if (next[i] == null)
                    {
                        continue;
                    }

                    (adj = next[i]).p = current;
                    adj.f = adj.g = 0;
                    adj.v = adj.x + adj.y * cols;

                    if (!list.ContainsKey(adj.v))
                    {
                        adj.f = (adj.g = current.g + distance(adj, current)) + distance(adj, end);
                        open[length++] = adj;
                        list[adj.v] = 1;
                    }
                }
            }
            else
            {
                i = length = 0;

                do
                {
                    Vector2Int point = new Vector2Int(current.x, current.y);
                    Result.Add(point);
                } while ((current = current.p) != null);

                Result.Reverse();
            }
        } while (length != 0);
    }
}