using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CombatUnit : TileActor
{
    public float hitPoints;

    public UnitMovement movement;

    public Vector2Int[] path;

    public SpriteChainRenderer renderedPath;

    public int HitPoints { get; }
}