using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A discrete grid of squares that actors can exist on and traverse.
/// </summary>
public sealed class TileGrid
{
    #region Immutable Fields
    private readonly int width;
    private readonly int height;
    private readonly float gridUnit;
    private readonly Vector2 worldTranslation;
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new tile grid with the size, scale, and world location transform.
    /// </summary>
    /// <param name="size">The width and height of the grid in tiles.</param>
    /// <param name="gridUnit">How many world units quate to a grid unit.</param>
    /// <param name="worldTranslation">The translation of this grid origin relative to the world origin.</param>
    public TileGrid(Vector2Int size, float gridUnit, Vector2 worldTranslation)
    {
        // Set up values for GridToWorld and WorldToGrid.
        width = size.x;
        height = size.y;
        this.gridUnit = gridUnit;
        this.worldTranslation = worldTranslation;
        // Initialize collections.
        Actors = new List<TileActor>();
        Commanders = new List<Commander>();
        Terrain = new Dictionary<Vector2Int, TileTerrain>();
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                Terrain.Add(new Vector2Int(x, y), default(TileTerrain));
        // Initialize A* collection.
        routeData = new RouteNodeData[width, height];
    }
    #endregion
    #region Properties
    /// <summary>
    /// The terrain on this tile grid.
    /// </summary>
    public Dictionary<Vector2Int, TileTerrain> Terrain { get; private set; }
    /// <summary>
    /// The actors on this tile grid.
    /// </summary>
    public List<TileActor> Actors { get; private set; }
    /// <summary>
    /// The commanders on this tile grid.
    /// </summary>
    public List<Commander> Commanders { get; private set; }
    /// <summary>
    /// The number of world units that equate to one grid tile.
    /// </summary>
    public float GridUnit { get { return gridUnit; } }
    #endregion
    #region World To Grid Conversion Methods
    /// <summary>
    /// Converts a world location to the nearest tile coordinate.
    /// </summary>
    /// <param name="worldLocation">The location in world space.</param>
    /// <returns>The coordinate on the grid.</returns>
    public Vector2Int WorldToGrid(Vector2 worldLocation)
    {
        return new Vector2Int
        {
            x = (int)(worldLocation.x - worldTranslation.x / gridUnit),
            y = (int)(worldLocation.y - worldTranslation.y / gridUnit)
        };
    }
    /// <summary>
    /// Converts a collection of world locations to the nearest tile coordinate.
    /// </summary>
    /// <param name="worldLocations">The locations in world space.</param>
    /// <returns>An array of coordinates on the grid.</returns>
    public Vector2Int[] WorldToGrid(IList<Vector2> worldLocations)
    {
        Vector2Int[] gridTiles = new Vector2Int[worldLocations.Count];
        for (int i = 0; i < worldLocations.Count; i++)
            gridTiles[i] = WorldToGrid(worldLocations[i]);
        return gridTiles;
    }
    #endregion
    #region Grid To World Conversion Methods
    /// <summary>
    /// Converts a grid tile to it's center position in world space.
    /// </summary>
    /// <param name="gridLocation">The grid coordinates.</param>
    /// <returns>A position in world space centered on this tile.</returns>
    public Vector2 GridToWorld(Vector2Int gridLocation)
    {
        return new Vector2
        {
            x = worldTranslation.x + (gridLocation.x + 0.5f) * gridUnit,
            y = worldTranslation.y + (gridLocation.y + 0.5f) * gridUnit
        };
    }
    /// <summary>
    /// Converts a collection of grid tiles to their center positions in world space.
    /// </summary>
    /// <param name="gridLocations">The grid coordinates.</param>
    /// <returns>An array of positions in world space centered on the tiles.</returns>
    public Vector2[] GridToWorld(IList<Vector2Int> gridLocations)
    {
        Vector2[] worldLocations = new Vector2[gridLocations.Count];
        for (int i = 0; i < gridLocations.Count; i++)
            worldLocations[i] = GridToWorld(gridLocations[i]);
        return worldLocations;
    }
    #endregion
    #region Actor Accessor Utility Methods
    /// <summary>
    /// Retrieves all actors that are currently occupying a tile.
    /// </summary>
    /// <param name="tile">The tile coordinates to check.</param>
    /// <returns>A list containing all present actors.</returns>
    public List<TileActor> GetActorsOnTile(Vector2Int tile)
    {
        // TODO this seems like a potentially slow
        // way to access actors; but a dictionary
        // introduces problems with staying up to date.
        List<TileActor> actorsOnTile = new List<TileActor>();
        foreach (TileActor actor in Actors)
            if (actor.Location == tile)
                actorsOnTile.Add(actor);
        return actorsOnTile;
    }
    /// <summary>
    /// Checks whether the given coordinates are valid for actors to exist on.
    /// </summary>
    /// <param name="tile">The tile coordinates.</param>
    /// <returns>True if an actor is able to occupy the tile.</returns>
    public bool DoesTileExist(Vector2Int tile)
    {
        // TODO: this also needs to take into account
        // tiles that are marked as void terrain.
        return tile.x >= 0 && tile.x < width
            && tile.y >= 0 && tile.y < height;
    }
    #endregion
    #region Movement Methods
    /// <summary>
    /// Determines all tiles that can be reached with the given move range.
    /// </summary>
    /// <param name="start">The starting tile coordinates.</param>
    /// <param name="type">The type of unit moving.</param>
    /// <param name="movePoints">The number of movement points available.</param>
    /// <returns>An array of all possible tile coordinates that can be reached.</returns>
    public Vector2Int[] CalculateMoveOptions(Vector2Int start, UnitType type, int movePoints)
    {
        // Set up a record of move points remaining
        // when making it to each grid tile.
        int[,] movePointsLeft = new int[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                movePointsLeft[x, y] = -1;
        // Initialize the start tile with full move points
        // and recursively explore movement options until
        // all possible routes are exhausted.
        movePointsLeft[start.x, start.y] = movePoints;
        ExploreRecursive(start);
        // Once the recursion finishes, scan the array
        // for all tiles that can be traveled to.
        List<Vector2Int> foundOptions = new List<Vector2Int>();
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (movePointsLeft[x, y] >= 0)
                    foundOptions.Add(new Vector2Int(x, y));
        return foundOptions.ToArray();

        // TODO this might not be the most efficient way to do this.
        // This solution is depth first which will result in a lot
        // of redundant branch checks. Make this breadth first.
        void ExploreRecursive(Vector2Int fromTile)
        {
            // Get the current state of movement.
            int pointsLeft =
                movePointsLeft[fromTile.x, fromTile.y];
            // Declare variables for checking branches.
            Vector2Int toTile;
            UnitTerrainData terrain;
            int pointsAfterMove;
            // Check each possible branch.
            foreach (Vector2Int direction in routeDirections)
            {
                // Check to ensure movement validity.
                toTile = fromTile + direction;
                if (DoesTileExist(toTile))
                {
                    terrain = Terrain[toTile][type];
                    if (terrain.canTraverse)
                    {
                        // Is this move possible and better
                        // than a previous recursion branch was
                        // able to accomplish?
                        pointsAfterMove = pointsLeft - terrain.moveCost;
                        if (pointsAfterMove >= 0
                            && pointsAfterMove > movePointsLeft[toTile.x, toTile.y])
                        {
                            // If so, then continue exploring this branch.
                            movePointsLeft[toTile.x, toTile.y] = pointsAfterMove;
                            ExploreRecursive(toTile);
                        }
                    }
                }
            }
        }
    }
    // TODO maybe this should be abstracted
    // and maybe only have heuristic/move-cost
    // functions defined here.
    #region A* Traversal
    /// <summary>
    /// Attempts to find a path connecting two points on the grid.
    /// </summary>
    /// <param name="start">The start point of the path.</param>
    /// <param name="end">The end point of the path.</param>
    /// <param name="unitType">Defines how the route handles terrain.</param>
    /// <param name="path">
    /// The output for the found path.
    /// Starts at the first tile to move to.
    /// If the function returns false the attempted path is still returned.
    /// </param>
    /// <returns>True if a valid complete path was found.</returns>
    public bool TryFindPath(Vector2Int start, Vector2Int end, UnitType unitType, int maxMoves, out Vector2Int[] path)
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
                path = UnwindPath(current, maxMoves);
                return true;
            }
            // Step into the current node by removing it.
            openNodes.Remove(current);
            closedNodes.Add(current);

            foreach (Vector2Int direction in routeDirections)
            {
                Vector2Int newTile = current + direction;
                // Check to see if this new tile is a valid option.
                // If not, immediately move it to the closed list.
                if (!DoesTileExist(newTile) || !Terrain[newTile][unitType].canTraverse)
                    closedNodes.Add(newTile);
                else if (!closedNodes.Contains(newTile))
                {
                    float newGScore = routeData[current.x, current.y].gScore +
                        CalculateMoveCost(newTile, unitType);
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
    // Functions.
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
    private float CalculateMoveCost(Vector2Int ontoTile, UnitType unitType)
    {
        // TODO this needs to consider terrain here!
        return 1;
    }
    private Vector2Int[] UnwindPath(Vector2Int node, int maxMoves)
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
        // Return the path in the correct order
        // and limited by the movement range.
        Vector2Int[] outPath = path.ToArray();
        Array.Resize(ref outPath, Mathf.Min(outPath.Length, maxMoves));
        return outPath;
    }
    #endregion
    #endregion
    #endregion
}
