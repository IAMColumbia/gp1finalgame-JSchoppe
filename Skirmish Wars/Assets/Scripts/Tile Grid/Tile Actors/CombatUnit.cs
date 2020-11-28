using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class CombatUnitState
{
    [Range(10f, 100f)] public float hitPoints;
    [Range(1, 100)] public int moveRange;
    public UnitMovement movement;

    public CombatUnitState()
    {
        hitPoints = 100f;
        moveRange = 5;
        movement = UnitMovement.Walking;
    }
}

public class CombatUnit : TileActor
{

    public CombatUnit(TileGrid grid, byte teamID, CombatUnitState initialState)
        : base(grid, teamID)
    {
        state = initialState;
    }

    public CombatUnitState state;

    public event Action<Vector2Int[]> PathChanged;

    public int HitPoints { get; }


    public override void OnClick()
    {
        movePath = new Vector2Int[] { location };
        PathChanged?.Invoke(movePath);
    }

    public override void OnDragNewTile(Vector2Int newTile)
    {
        base.OnDragNewTile(newTile);

        bool intersectsOldPath = false;
        for (int i = 0; i < movePath.Length; i++)
        {
            if (newTile == movePath[i])
            {
                Array.Resize(ref movePath, i + 1);
                intersectsOldPath = true;
                break;
            }
        }
        if (!intersectsOldPath && movePath.Length <= state.moveRange)
        {
            Array.Resize(ref movePath, movePath.Length + 1);
            movePath[movePath.Length - 1] = newTile;
        }
        PathChanged?.Invoke(movePath);
    }


    private Vector2Int[] movePath;
    public Vector2Int[] MovePath
    {
        get { return movePath; }
    }

}