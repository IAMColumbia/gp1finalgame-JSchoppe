using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Commander : MonoBehaviour
{
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
            List<TileActor> actors = grid.actors[gridPosition];
            foreach (TileActor actor in actors)
            {
                if (actor.Team == team)
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


    protected virtual void Awake()
    {
        controller.Clicked += OnClick;
        controller.Released += OnRelease;
        controller.Drag += OnDrag;
    }


    private IEnumerator WhileDragging()
    {
        throw new NotImplementedException();
    }

    public Team team;
    public TileGrid grid;
    public List<CombatUnit> units;
    public CursorController controller;
}