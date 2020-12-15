using System.Collections.Generic;
using UnityEngine; // Only used for common Vector2/Vector2Int functionality.

/// <summary>
/// Base class for all commanders.
/// </summary>
public abstract class Commander
{
    #region Parameters
    private const int MAX_CURSOR_STEPS = 50;
    #endregion
    #region Abstract Constructor
    public Commander(byte teamID, TileGrid grid, CursorController controller)
    {
        this.teamID = teamID;
        this.grid = grid;
        this.controller = controller;
        // Scan the grid to populate this commanders
        // units into a local collection.
        units = new List<CombatUnit>();
        foreach (CombatUnit unit in grid.Actors)
            if (unit.TeamID == teamID)
                units.Add(unit);
        // Bind to the cursor events.
        controller.PrimaryPressed += OnClick;
        controller.PrimaryReleased += OnRelease;
        controller.PrimaryDragging += OnDrag;
        // Assign the team ID to the cursor (informs render style).
        controller.TeamID = teamID;
    }
    #endregion
    // TODO encapsulate more of this and add XML
    // comments on exposed properties.
    public byte teamID;
    public TileGrid grid;
    public List<CombatUnit> units;
    public CursorController controller;
    protected TileActor targetedActor;
    private Vector2Int currentTile;
    #region Start/Stop Command Phase
    public virtual void OnCommandPhaseBegin()
    {
        // For each unit calculate their move options
        // so that this does not need to be done constantly.
        foreach (CombatUnit unit in units)
            unit.RefreshMoveOptions();
    }
    // Implement this if the sub class commander needs
    // to be interupted somehow at the end of the command
    // phase.
    public virtual void OnCommandPhaseEnd() { }
    #endregion
    #region High Level Drag Implementation
    protected virtual void OnClick(Vector2 location)
    {
        // See if there is a targeted actor to select
        // at the clicked location.
        targetedActor = null;
        Vector2Int gridPosition = grid.WorldToGrid(location);
        if (grid.DoesTileExist(gridPosition))
        {
            // In theory there should only be one unit on a tile
            // at any given time during the command phase. TODO
            // may want to just check the first actor, but not doing
            // this for now in case non-unit actors are to be implemented.
            foreach (TileActor actor in grid.GetActorsOnTile(gridPosition))
            {
                if (actor.TeamID == teamID)
                {
                    currentTile = gridPosition;
                    targetedActor = actor;
                    // Tell this actor that it has been clicked.
                    targetedActor.OnClick();
                    break;
                }
            }
        }
    }
    protected virtual void OnDrag(Vector2 location)
    {
        // Check to see if the dragged cursor is now
        // hovering over a new tile. Only if there is
        // a targeted actor that it would effect.
        if (targetedActor != null)
        {
            // Where is the cursor right now?
            Vector2Int newTile =
                 grid.WorldToGrid(location);
            // What direction do we have to walk in to reach this tile?
            int xStep = (newTile.x - currentTile.x > 0) ? 1 : -1;
            int yStep = (newTile.y - currentTile.y > 0) ? 1 : -1;
            // Step towards the new tile, notifying the targeted actor
            // of each tile that we cross along the way.
            int step = 0;
            while (currentTile != newTile)
            {
                // Hard limit put in place may break behaviour on cursor
                // teleportation. Meant to prevent possible edge cases
                // where the requested tile cannot be walked to.
                if (++step > MAX_CURSOR_STEPS)
                    break;
                // Walk tiles along each axis.
                // This will generally infer a pattern where the cursor
                // moves along a diagonally then flattens, this does not
                // necassarily replicate the linear path of the cursor.
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
    #endregion
}
