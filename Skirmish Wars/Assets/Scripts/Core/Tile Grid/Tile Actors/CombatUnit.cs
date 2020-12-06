using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

#region Exposed Enums
/// <summary>
/// Defines how damage tables apply to units.
/// </summary>
public enum UnitType : byte
{
    FootSoldier,
    Tank
}
#endregion
#region Exposed Structs/Classes
[Serializable]
public class CombatUnitState
{
    [Range(0.1f, 1.0f)] public float hitPoints;
    [Range(1, 100)] public int moveRange;
    public UnitType type;
    #region Constructors
    public CombatUnitState()
    {
        hitPoints = 1f;
        moveRange = 5;
    }
    #endregion
}
#endregion

public class CombatUnit : TileActor
{

    public CombatUnit(TileGrid grid, byte teamID, CombatUnitState initialState)
        : base(grid, teamID)
    {
        hitPoints = initialState.hitPoints;
        moveRange = initialState.moveRange;
        type = initialState.type;
        movePath = new LinkedList<Vector2Int>();
    }

    public float hitPoints;
    public int moveRange;
    public UnitType type;

    public float HitPoints
    {
        get { return hitPoints; }
        set
        {
            hitPoints = value;
            HitPointsChanged?.Invoke(value);
        }
    }

    public bool PathShown
    {
        set
        {
            PathShownChanged?.Invoke(value);
        }
    }

    // TODO this is a hotfix (subverts structure).
    public event Action<bool> FocusChanged;
    public bool HasFocus
    {
        set { FocusChanged?.Invoke(value); }
    }

    public event Action<Vector2Int[]> PathChanged;

    public event Action<float> MovementAnimating;

    public event Action<float> HitPointsChanged;

    public event Action<bool> PathShownChanged;

    public void AnimateMovement(float interpolant)
    {
        MovementAnimating?.Invoke(interpolant);
    }

    public Vector2Int[] PossibleDestinations { get; private set; }

    public void RefreshMoveOptions()
    {
        PossibleDestinations = 
            grid.CalculateMoveOptions(Location, type, moveRange);
    }

    public override void OnClick()
    {
        base.OnClick();
        movePath = new LinkedList<Vector2Int>();
        movePath.AddLast(location);
        PathChanged?.Invoke(movePath.ToArray());
    }

    public override void OnDragNewTile(Vector2Int newTile)
    {
        // TODO this whole method seems inneficient and
        // could probably be rethought through.

        base.OnDragNewTile(newTile);
        int remainingMoves = CalculateRemainingMoves();

        // Check to see if the next tile can
        // be moved on by this unit type.
        UnitTerrainData terrain = grid.Terrain[newTile][type];
        if (terrain.canTraverse)
        {
            // Use A* to infer the path to the new tile.
            // This step is required if the player drags through
            // non-navigable terrain; it will infer the path
            // around the terrain.
            if (grid.TryFindPath(movePath.Last.Value, newTile, type, remainingMoves,
                out Vector2Int[] addedPath))
            {
                // Add the inferred path until there is not
                // enough remaining movement.
                foreach (Vector2Int tile in addedPath)
                {
                    remainingMoves -= grid.Terrain[tile][type].moveCost;
                    if (remainingMoves >= 0)
                        CheckIntersectionAndJoin(tile);
                }
            }
        }
        PathChanged?.Invoke(movePath.ToArray());

        void CheckIntersectionAndJoin(Vector2Int joinTile)
        {
            // Check to see if the path is self intersecting.
            // This ensures that paths can't stack on themselves
            // which would make the scene very hard to comprehend.
            bool didIntersect = false;
            if (joinTile == Location)
            {
                didIntersect = true;
                movePath.Clear();
            }
            else
            {
                foreach (Vector2Int tile in movePath)
                {
                    if (joinTile == tile)
                    {
                        movePath.Truncate(tile);
                        didIntersect = true;
                        break;
                    }
                }
            }
            movePath.AddLast(joinTile);
            if (didIntersect)
                remainingMoves = CalculateRemainingMoves();
        }
        int CalculateRemainingMoves()
        {
            int movesLeft = moveRange;
            LinkedListNode<Vector2Int> node = movePath.First;
            while (node.Next != null)
            {
                node = node.Next;
                movesLeft -= grid.Terrain[node.Value][type].moveCost;
            }
            return movesLeft;
        }
    }


    private LinkedList<Vector2Int> movePath;
    public LinkedList<Vector2Int> MovePath
    {
        get { return movePath; }
        set
        {
            movePath = value;
            PathChanged?.Invoke(movePath.ToArray());
        }
    }
}