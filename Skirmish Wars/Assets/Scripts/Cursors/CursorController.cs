using System;
using UnityEngine;

/// <summary>
/// Base class for cursor controllers.
/// </summary>
public abstract class CursorController
{
    #region Renderer Events
    /// <summary>
    /// Called when the cursor render state changes.
    /// </summary>
    public event Action<RenderedCursorState> RenderStateChanged;
    /// <summary>
    /// Called when the cursor location changes.
    /// </summary>
    public event Action<Vector2> LocationChanged;
    #endregion
    #region Cursor Events
    /// <summary>
    /// Called when this cursor clicks.
    /// </summary>
    public abstract event Action<Vector2> Clicked;
    /// <summary>
    /// Called when this cursor releases.
    /// </summary>
    public abstract event Action<Vector2> Released;
    /// <summary>
    /// Called every update cycle while the controller is dragging.
    /// </summary>
    public event Action<Vector2> Drag;
    #endregion
    #region Common Fields
    protected TileGrid grid;
    private RenderedCursorState renderState;
    private Vector2 worldLocation;
    #endregion
    #region Abstract Constructors
    /// <summary>
    /// Creates the base cursor controller with the grid context.
    /// </summary>
    /// <param name="grid">The tile grid that this controller is relative to.</param>
    public CursorController(TileGrid grid)
    {
        this.grid = grid;
        worldLocation = Vector2.zero;
        // Bind to subclass events so the drag
        // event can be handled on this level.
        Clicked += OnClick;
        Released += OnRelease;
    }
    #endregion
    #region Properties
    /// <summary>
    /// The world location of the cursor.
    /// </summary>
    protected Vector2 WorldLocation
    {
        get { return worldLocation; }
        set
        {
            // Don't update cursor if it is still.
            if (worldLocation != value)
            {
                worldLocation = value;
                LocationChanged?.Invoke(value);
            }
        }
    }
    /// <summary>
    /// The visual state of the rendered cursor.
    /// </summary>
    protected RenderedCursorState RenderState
    {
        get { return renderState; }
        set
        {
            // Don't update cursor if the state hasn't changed.
            if (renderState != value)
            {
                renderState = value;
                RenderStateChanged?.Invoke(value);
            }
        }
    }
    /// <summary>
    /// The grid that this cursor controller is relative to.
    /// </summary>
    public TileGrid Grid { get { return grid; } }
    #endregion
    #region Drag Implementation
    private void OnClick(Vector2 location)
    {
        RenderState = RenderedCursorState.Held;
        UpdateContext.Update += OnDragUpdate;
    }
    private void OnRelease(Vector2 location)
    {
        UpdateContext.Update -= OnDragUpdate;
        RenderState = RenderedCursorState.Active;
    }
    private void OnDragUpdate()
    {
        Drag?.Invoke(worldLocation);
    }
    #endregion
}
