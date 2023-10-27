using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform startTransform;
    public Transform goalTransform;
    public Vector2 gridSize;
    public LayerMask obstacleLayer;
    public static Pathfinding Instance;
    

    private Tile[,] grid;

    private void Awake()
    {
        Instance = this;
        InitializeGrid();
        FindPath(startTransform.position, goalTransform.position);
    }

    void InitializeGrid()
    {
        gridSize.x = GridManager.Instance.width;
        gridSize.y = GridManager.Instance.height;

        grid = new Tile[(int)gridSize.x, (int)gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPoint = new Vector3(x, y, 0);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, 0.1f, obstacleLayer);
                grid[x, y] = new Tile { x = x, y = y, isWalkable = walkable };
            }
        }
    }

    List<Tile> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Tile startTile = WorldToTile(startPos);
        Tile goalTile = WorldToTile(targetPos);

        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tile currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentTile.FCost || openSet[i].FCost == currentTile.FCost && openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == goalTile)
            {
                // Path found
                return RetracePath(startTile, goalTile);
            }

            foreach (Tile neighbor in GetNeighbors(currentTile))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentTile.gCost + GetDistance(currentTile, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, goalTile);
                    neighbor.parent = currentTile;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        // Path not found
        return null;
    }

    List<Tile> RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Reverse();
        return path;
    }

    List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = tile.x + x;
                int checkY = tile.y + y;

                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    Tile WorldToTile(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);
        return grid[x, y];
    }

    int GetDistance(Tile tileA, Tile tileB)
    {
        int distX = Mathf.Abs(tileA.x - tileB.x);
        int distY = Mathf.Abs(tileA.y - tileB.y);

        return distX + distY;
    }
}







