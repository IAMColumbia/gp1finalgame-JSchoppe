using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Commander
{

    public Commander(byte teamID, TileGrid grid, CursorController controller)
    {
        this.teamID = teamID;
        this.grid = grid;
        this.controller = controller;
        controller.TeamID = teamID;
        controller.Clicked += OnClick;
        controller.Released += OnRelease;
        controller.Drag += OnDrag;
    }

    public byte teamID;
    public TileGrid grid;
    public List<CombatUnit> units;
    public CursorController controller;

    protected virtual void OnCommandPhaseBegin() { }
    protected virtual void OnCommandPhaseEnd() { }

    private TileActor targetedActor;
    private Vector2Int currentTile;
    protected virtual void OnClick(Vector2 location)
    {
        targetedActor = null;
        Vector2Int gridPosition = grid.WorldToGrid(location);
        if (grid.DoesTileExist(gridPosition))
        {
            currentTile = gridPosition;
            List<TileActor> actors = grid.GetActorsOnTile(gridPosition);
            foreach (TileActor actor in actors)
            {
                if (actor.TeamID == teamID)
                {
                    targetedActor = actor;
                    actor.OnClick();
                    break;
                }
            }
        }
    }
    protected virtual void OnDrag(Vector2 location)
    {
        if (targetedActor != null)
        {
            Vector2Int newTile =
                 grid.WorldToGrid(location);

            int xStep = (newTile.x - currentTile.x > 0) ? 1 : -1;
            int yStep = (newTile.y - currentTile.y > 0) ? 1 : -1;

            int MAX_STEPS = 50;
            int STEP = 0;
            while (currentTile != newTile)
            {
                STEP++;
                if (STEP > MAX_STEPS)
                {
                    break;
                }

                if (currentTile.x != newTile.x)
                {
                    currentTile.x += xStep;
                    targetedActor.OnDragNewTile(currentTile);
                }
                if (currentTile.y != newTile.y)
                {
                    currentTile.y += yStep;
                    targetedActor.OnDragNewTile(currentTile);
                }
            }
        }
    }
    protected virtual void OnRelease(Vector2 location)
    {
        if (targetedActor != null)
            targetedActor.OnRelease(grid.WorldToGrid(location));
    }
}
