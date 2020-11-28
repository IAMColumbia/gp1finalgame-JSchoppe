using System;
using UnityEngine; // Only used for Vector2Int.

/// <summary>
/// Base class for actors that move on the grid.
/// </summary>
public abstract class TileActor
{
    #region Events
    public event Action<Team> TeamChanged;
    public event Action<Vector2Int> Teleported;
    #endregion
    #region Actor Common Fields
    protected byte teamID;
    protected TileGrid grid;
    protected Vector2Int location;
    #endregion
    #region Abstract Constructors
    /// <summary>
    /// Creates a new tile actor on the given grid and team ID.
    /// </summary>
    /// <param name="grid">The grid this actor resides in (cannot change once set).</param>
    /// <param name="teamID">The team ID for this actor.</param>
    public TileActor(TileGrid grid, byte teamID)
    {
        this.grid = grid;
        this.teamID = teamID;
    }
    #endregion
    #region Properties
    /// <summary>
    /// This actors team identity.
    /// </summary>
    public byte TeamID
    {
        get { return teamID; }
        set
        {
            teamID = value;
            TeamChanged?.Invoke(TeamsSingleton.FromID(value));
        }
    }
    /// <summary>
    /// The grid that this actor resides on.
    /// </summary>
    public TileGrid Grid { get { return grid; } }
    /// <summary>
    /// The unit grid location. Teleports the unit if set.
    /// </summary>
    public Vector2Int Location
    {
        get { return location; }
        set
        {
            location = value;
            Teleported?.Invoke(value);
        }
    }
    #endregion
    #region Optional Subclass Implementations
    /// <summary>
    /// Notifies the actor that a cursor has requested focus.
    /// </summary>
    public virtual void OnClick() { }
    /// <summary>
    /// Notifies the actor that a cursor has moved to a new tile.
    /// </summary>
    /// <param name="newTile">The new tile.</param>
    public virtual void OnDragNewTile(Vector2Int newTile) { }
    /// <summary>
    /// Notifies the actor that they no longer have focus.
    /// </summary>
    /// <param name="finalTile">The final cursor location.</param>
    public virtual void OnRelease(Vector2Int finalTile) { }
    #endregion
}
