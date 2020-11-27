using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TileGrid : MonoBehaviour
{
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float gridUnit = 1f;

    private void OnValidate()
    {
        width.Clamp(1, int.MaxValue);
        height.Clamp(1, int.MaxValue);
        gridUnit.Clamp(0.1f, float.MaxValue);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        // Draw the grid into the scene view for reference.
        Vector3 offset = height * Vector3.up * gridUnit;
        for (int x = 0; x <= width; x++)
        {
            Vector3 along = transform.position + Vector3.right * x * gridUnit;
            Debug.DrawLine(along, along + offset);
        }
        offset = width * Vector3.right * gridUnit;
        for (int y = 0; y <= height; y++)
        {
            Vector3 along = transform.position + Vector3.up * y * gridUnit;
            Debug.DrawLine(along, along + offset);
        }
    }

    private void Awake()
    {
        // Initialize actor directory.
        actors = new Dictionary<Vector2Int, List<TileActor>>();
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                actors.Add(new Vector2Int(x, y), new List<TileActor>());
        // Initialize A* collection.
        routeData = new RouteNodeData[width, height];
    }

    public void ParseSceneUnitsOntoGrid()
    {
        foreach (CombatUnit unit in FindObjectsOfType<CombatUnit>())
        {
            Vector2Int location = WorldToGrid(unit.transform.position);
            if (DoesTileExist(location))
            {
                actors[location].Add(unit);
            }
        }
    }


    public Vector2Int WorldToGrid(Vector2 worldLocation)
    {
        return new Vector2Int
        {
            x = (int)(worldLocation.x - transform.position.x / gridUnit),
            y = (int)(worldLocation.y - transform.position.y / gridUnit)
        };
    }

    public Vector2Int[] WorldToGrid(IList<Vector2> worldLocations)
    {
        Vector2Int[] gridTiles = new Vector2Int[worldLocations.Count];
        for (int i = 0; i < worldLocations.Count; i++)
            gridTiles[i] = WorldToGrid(worldLocations[i]);
        return gridTiles;
    }

    public Vector2 GridToWorld(Vector2Int gridLocation)
    {
        return new Vector2
        {
            x = transform.position.x + (gridLocation.x + 0.5f) * gridUnit,
            y = transform.position.y + (gridLocation.y + 0.5f) * gridUnit
        };
    }
    public Vector2[] GridToWorld(IList<Vector2Int> gridLocations)
    {
        Vector2[] worldLocations = new Vector2[gridLocations.Count];
        for (int i = 0; i < gridLocations.Count; i++)
            worldLocations[i] = GridToWorld(gridLocations[i]);
        return worldLocations;
    }

    public Dictionary<Vector2Int, List<TileActor>> actors;

    public Dictionary<Vector2Int, TileTerrain> terrain;

    public List<Commander> commanders;

    public bool DoesTileExist(Vector2Int tile)
    {
        return tile.x >= 0 && tile.x < width
            && tile.y >= 0 && tile.y < height;
    }


    public Vector2Int[] CalculateMoveOptions(Vector2Int start, UnitMovement mode, int distance)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Attempts to find a path connecting two points on the grid.
    /// </summary>
    /// <param name="start">The start point of the path.</param>
    /// <param name="end">The end point of the path.</param>
    /// <param name="moveType">Defines how the route handles terrain.</param>
    /// <param name="path">
    /// The output for the found path.
    /// Starts at the first tile to move to.
    /// If the function returns false the attempted path is still returned.
    /// </param>
    /// <returns>True if a valid complete path was found.</returns>
    public bool TryFindPath(Vector2Int start, Vector2Int end, UnitMovement moveType, int maxMoves, out Vector2Int[] path)
    {
        #region Already At End Edge Case
        if (start == end)
        {
            path = new Vector2Int[0];
            return true;
        }
        #endregion
        #region No Moves Edge Case
        if (maxMoves <= 0)
        {
            path = new Vector2Int[0];
            return false;
        }
        #endregion

        // Clear previous pathfinding state.	
        ResetNodeData();
        // Initialize the collection for A*.	
        List<Vector2Int> openNodes = new List<Vector2Int>();
        List<Vector2Int> closedNodes = new List<Vector2Int>();
        // Initialize the starting node.	
        openNodes.Add(start);
        routeData[start.x, start.y].gScore = 0f;
        routeData[start.x, start.y].hScore = CalculateHeuristic(start, end);
        // This is the sentinel for unwrapping the path.
        routeData[start.x, start.y].parent = new Vector2Int(-1, -1);

        // Start the A* algorithm.
        while (openNodes.Count > 0)
        {
            // Find the best f score in the open nodes.	
            Vector2Int current = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (routeData[openNodes[i].x, openNodes[i].y].fScore
                    < routeData[current.x, current.y].fScore)
                {
                    current = openNodes[i];
                }
            }
            // Return the path if the end has been found.	
            if (current == end)
            {
                path = UnwindPath(current);
                return true;
            }
            // Step into the current node by removing it.
            openNodes.Remove(current);
            closedNodes.Add(current);

            foreach (Vector2Int direction in routeDirections)
            {
                Vector2Int newTile = current + direction;
                // TODO this also needs to hard check whether
                // this tile is navigatable at all.
                if (DoesTileExist(newTile) && !closedNodes.Contains(newTile))
                {
                    float newGScore = routeData[current.x, current.y].gScore +
                        CalculateMoveCost(newTile, moveType);
                    if (newGScore < routeData[newTile.x, newTile.y].gScore)
                    {
                        // If this is a new best path to this node,	
                        // add it to the open nodes and calculate the heuristic.	
                        routeData[newTile.x, newTile.y].gScore = newGScore;
                        routeData[newTile.x, newTile.y].parent = current;
                        // If this is a new node, calculate its heuristic.
                        if (!openNodes.Contains(newTile))
                        {
                            routeData[newTile.x, newTile.y].hScore =
                                CalculateHeuristic(newTile, end);
                            openNodes.Add(newTile);
                        }
                    }
                }
            }
        }
        // A* pathfinding failed to find a path.	
        path = new Vector2Int[0];
        return false;
    }


    #region A* Utilities
    // TODO maybe these A* components should
    // be abstracted.
    private readonly Vector2Int[] routeDirections =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.right,
        Vector2Int.left
    };
    private struct RouteNodeData
    {
        public Vector2Int parent;
        public float gScore;
        public float hScore;
        public float fScore { get { return gScore + hScore; } }
    }
    private RouteNodeData[,] routeData;
    // Functions
    private void ResetNodeData()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                routeData[x, y].hScore = 0f;
                routeData[x, y].gScore = float.MaxValue / 2f;
            }
        }
    }
    private float CalculateHeuristic(Vector2Int nodeA, Vector2Int nodeB)
    {
        // Uses manhattan distance.
        return Mathf.Abs(nodeA.x - nodeB.x)
            + Mathf.Abs(nodeA.y - nodeB.y);
    }
    private float CalculateMoveCost(Vector2Int ontoTile, UnitMovement moveType)
    {
        // TODO this needs to consider terrain here!
        return 1;
    }
    private Vector2Int[] UnwindPath(Vector2Int node)
    {
        // Unwind the path using the parent field.
        Stack<Vector2Int> path = new Stack<Vector2Int>();
        path.Push(node);
        // Watch for sentinel to exit.
        while (routeData[node.x, node.y].parent.x != -1)
        {
            node = routeData[node.x, node.y].parent;
            path.Push(node);
        }
        // Once exited remove the starting node.
        path.Pop();
        // Return the path in the correct order.	
        return path.ToArray();
    }
    #endregion
}
