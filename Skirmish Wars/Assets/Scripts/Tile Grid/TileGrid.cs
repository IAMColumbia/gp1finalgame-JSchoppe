using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TileGrid
{
    public int width, height;

    public Dictionary<Vector2Int, List<TileActor>> actors;

    public Dictionary<Vector2Int, TileTerrain> terrain;

    public List<Commander> commanders;

    public bool DoesTileExist(Vector2Int tile)
    {
        throw new NotImplementedException();
    }

    public Vector2Int[] CalculateMoveOptions(Vector2Int start, UnitMovement mode, int distance)
    {
        throw new NotImplementedException();
    }
}