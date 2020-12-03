using System;
using UnityEngine; // Only used for common Vector2 struct.

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
    /// <summary>
    /// Called when the controlling commander's team changes.
    /// </summary>
    public event Action<Team> TeamChanged;
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
    private bool isDragging;
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
        isDragging = false;
        // Bind to subclass events so the drag
        // event can be handled on this level.
        Clicked += OnClick;
        Released += OnRelease;
        // Set enabled by default.
        IsEnabled = true;
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
    public RenderedCursorState RenderState
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
    /// Updates the visual components of this cursor controller.
    /// </summary>
    public byte TeamID
    {
        set { TeamChanged?.Invoke(TeamsSingleton.FromID(value)); }
    }
    /// <summary>
    /// Enables or disables the controller click-drag-release events.
    /// </summary>
    public abstract bool IsEnabled { get; set; }
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
        isDragging = true;
    }
    private void OnRelease(Vector2 location)
    {
        UpdateContext.Update -= OnDragUpdate;
        RenderState = RenderedCursorState.Active;
        isDragging = false;
    }
    private void OnDragUpdate()
    {
        Drag?.Invoke(worldLocation);
    }
    #endregion
    #region Interruption Implementation
    /// <summary>
    /// Interrupts the current controller action
    /// and reverts it to its default state.
    /// </summary>
    protected virtual void InterruptController()
    {
        // Clear drag state.
        if (isDragging)
        {
            UpdateContext.Update -= OnDragUpdate;
            isDragging = false;
        }
        // Change the rendered cursor to ghost.
        RenderState = RenderedCursorState.Ghost;
    }
    #endregion
}
