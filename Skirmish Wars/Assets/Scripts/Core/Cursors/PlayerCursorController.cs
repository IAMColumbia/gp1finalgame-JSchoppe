using System;
using UnityEngine;
// TODO figure out a way to decouple this script
// from Unity (new input system requires monobehaviour).
using SkirmishWars.Unity;

/// <summary>
/// Cursor controller that follows the mouse device.
/// </summary>
public sealed class PlayerCursorController : CursorController
{
    #region Cursor Events
    /// <summary>
    /// Called when this controller "clicks".
    /// Passes through the world space coordinates of the click.
    /// </summary>
    public override event Action<Vector2> PrimaryPressed;
    /// <summary>
    /// Called when this controller releases a "click".
    /// Passes through the world space coordinates of the released click.
    /// </summary>
    public override event Action<Vector2> PrimaryReleased;
    /// <summary>
    /// Called when this controller "right clicks".
    /// Passes through the world space coordinates of the click.
    /// </summary>
    public override event Action<Vector2> SecondaryPressed;
    /// <summary>
    /// Called when this controller releases a "right click".
    /// Passes through the world space coordinates of the click.
    /// </summary>
    public override event Action<Vector2> SecondaryReleased;
    #endregion
    #region Fields
    private MouseListener mouse;
    private Camera camera;
    private bool inDrag;
    private bool isEnabled;
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new player cursor controller.
    /// </summary>
    /// <param name="grid">The grid that this controller is relative to.</param>
    /// <param name="mouse">The mouse listener that this controller uses.</param>
    /// <param name="camera">The camera that this controller is relative to.</param>
    public PlayerCursorController(TileGrid grid, MouseListener mouse, Camera camera)
        : base(grid)
    {
        this.camera = camera;
        this.mouse = mouse;
        UpdateContext.Update += OnUpdate;
        // Bubble events from the mouse listener.
        // TODO this setup seems not pog; maybe
        // a better way to route events here.
        mouse.LeftClicked += ConvertPrimaryClicked;
        mouse.LeftReleased += ConvertPrimaryReleased;
        mouse.RightClicked += ConvertSecondaryClicked;
        mouse.RightReleased += ConvertSecondaryReleased;
    }
    #endregion
    #region Properties
    /// <summary>
    /// Enables or disables click events and current drag action.
    /// </summary>
    public override bool IsEnabled
    {
        get { return isEnabled; }
        set
        {
            isEnabled = value;
            if (!isEnabled && inDrag)
            {
                PrimaryReleased?.Invoke(camera.ScreenToWorldPoint(mouse.ScreenLocation));
                InterruptController();
                inDrag = false;
            }
        }
    }
    #endregion
    #region Mouse Listeners
    private void ConvertPrimaryClicked(Vector2 location)
    {
        // Convert from screen space to world space.
        if (IsEnabled)
        {
            inDrag = true;
            PrimaryPressed?.Invoke(camera.ScreenToWorldPoint(location));
        }
    }
    private void ConvertPrimaryReleased(Vector2 location)
    {
        // Convert from screen space to world space.
        if (IsEnabled)
        {
            inDrag = false;
            PrimaryReleased?.Invoke(camera.ScreenToWorldPoint(location));
        }
    }
    private void ConvertSecondaryClicked(Vector2 location)
    {
        // Convert from screen space to world space.
        if (IsEnabled)
        {
            inDrag = true;
            SecondaryPressed?.Invoke(camera.ScreenToWorldPoint(location));
        }
    }
    private void ConvertSecondaryReleased(Vector2 location)
    {
        // Convert from screen space to world space.
        if (IsEnabled)
        {
            inDrag = false;
            SecondaryReleased?.Invoke(camera.ScreenToWorldPoint(location));
        }
    }
    #endregion
    #region Update Cursor Location
    private void OnUpdate()
    {
        // Convert the mouse screen space coords to world space
        // using the given camera.
        WorldLocation = 
            camera.ScreenToWorldPoint(mouse.ScreenLocation);
    }
    #endregion
}
