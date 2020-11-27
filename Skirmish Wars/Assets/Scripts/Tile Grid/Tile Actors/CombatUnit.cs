using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CombatUnit : TileActor
{
    public float hitPoints;

    public int moveRange;

    public UnitMovement movement;

    public event Action<Vector2Int[]> PathChanged;

    public int HitPoints { get; }


    public override void OnClick()
    {
        movePath = new Vector2Int[]
        {
            Grid.WorldToGrid(transform.position)
        };
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
        if (!intersectsOldPath && movePath.Length <= moveRange)
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