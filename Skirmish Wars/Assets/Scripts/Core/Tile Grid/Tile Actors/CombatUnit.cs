using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region Exposed Enums
/// <summary>
/// Defines how terrain rules apply to a unit.
/// </summary>
public enum UnitMovement : byte
{
    Walking, Wheels, Flight
}
/// <summary>
/// Defines how damage tables apply to units.
/// </summary>
public enum UnitType : byte
{
    FootSoldier
}
#endregion
#region Exposed Structs/Classes
[Serializable]
public class CombatUnitState
{
    [Range(0.1f, 1.0f)] public float hitPoints;
    [Range(1, 100)] public int moveRange;
    public UnitMovement movement;
    #region Constructors
    public CombatUnitState()
    {
        hitPoints = 1f;
        moveRange = 5;
        movement = UnitMovement.Walking;
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
        movement = initialState.movement;
        movePath = new LinkedList<Vector2Int>();
    }

    public float hitPoints;
    public int moveRange;
    public UnitMovement movement;

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


    public event Action<Vector2Int[]> PathChanged;

    public event Action<float> MovementAnimating;

    public event Action<float> HitPointsChanged;

    public event Action<bool> PathShownChanged;

    public void AnimateMovement(float interpolant)
    {
        MovementAnimating?.Invoke(interpolant);
    }


    public override void OnClick()
    {
        movePath = new LinkedList<Vector2Int>();
        movePath.AddLast(location);
        PathChanged?.Invoke(movePath.ToArray());
    }

    public override void OnDragNewTile(Vector2Int newTile)
    {
        base.OnDragNewTile(newTile);

        bool intersectsOldPath = false;
        foreach (Vector2Int tile in movePath)
        {
            if (newTile == tile)
            {
                movePath.Truncate(tile);
                intersectsOldPath = true;
                break;
            }
        }
        if (!intersectsOldPath && movePath.Count <= moveRange)
            movePath.AddLast(newTile);

        PathChanged?.Invoke(movePath.ToArray());
    }


    private LinkedList<Vector2Int> movePath;
    public LinkedList<Vector2Int> MovePath
    {
        set
        {
            movePath = value;
            PathChanged?.Invoke(movePath.ToArray());
        }
        get { return movePath; }
    }
}