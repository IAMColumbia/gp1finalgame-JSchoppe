using System;
using UnityEngine;

/// <summary>
/// Visual state for a cursor.
/// </summary>
public enum RenderedCursorState : byte
{
    Active, Held, Hidden, Ghost
}

/// <summary>
/// Renders a player cursor onto the scene.
/// </summary>
public sealed class CursorRenderer : MonoBehaviour
{
    #region Inspector Fields
    [Tooltip("Renders the pointer on the screen.")]
    [SerializeField] private SpriteRenderer cursorSprite = null;
    [Tooltip("Renders the highlighted tile on the screen.")]
    [SerializeField] private SpriteRenderer highlightSprite = null;
    [Tooltip("Controls the animation of the highlight.")]
    [SerializeField] private Animator highlightAnimator = null;
    [Range(0f, 1f)][Tooltip("Cursor opacity when the cursor is in ghost state.")]
    [SerializeField] private float ghostOpacity = 0.5f;
    #endregion
    #region Private Fields
    private int cursorActiveProperty;
    private CursorController drivingController;
    #endregion
    #region Properties
    /// <summary>
    /// The cursor controller that drives this renderer.
    /// </summary>
    public CursorController DrivingController
    {
        set
        {
            // Unbind a previous driver.
            if (drivingController != null)
            {
                drivingController.RenderStateChanged -= OnRenderStateChanged;
                drivingController.LocationChanged -= OnLocationChanged;
            }
            // Bind to the new driving controller.
            value.RenderStateChanged += OnRenderStateChanged;
            value.LocationChanged += OnLocationChanged;
            drivingController = value;
        }
    }
    /// <summary>
    /// Sets the visual state of this cursor.
    /// </summary>
    public RenderedCursorState RenderState
    {
        set
        {
            // Choose a value for the sprite alphas.
            float alpha;
            switch (value)
            {
                case RenderedCursorState.Hidden:
                    alpha = 0f; break;
                case RenderedCursorState.Ghost:
                    alpha = ghostOpacity; break;
                case RenderedCursorState.Active:
                case RenderedCursorState.Held:
                    alpha = 1f; break;
                default:
                    throw new NotImplementedException();
            }
            // Apply the new alpha channel.
            cursorSprite.color = new Color
            {
                r = cursorSprite.color.r,
                g = cursorSprite.color.g,
                b = cursorSprite.color.b,
                a = alpha
            };
            highlightSprite.color = new Color
            {
                r = highlightSprite.color.r,
                g = highlightSprite.color.g,
                b = highlightSprite.color.b,
                a = alpha
            };
            // Set the animator state based on the interaction state.
            if (value == RenderedCursorState.Held)
                highlightAnimator.SetBool(cursorActiveProperty, true);
            else
                highlightAnimator.SetBool(cursorActiveProperty, false);
        }
    }
    /// <summary>
    /// The pointer location of the cursor in world space.
    /// </summary>
    public Vector2 Location
    {
        get { return cursorSprite.transform.position; }
        set { cursorSprite.transform.position = value; }
    }
    /// <summary>
    /// The location of the center of the highlighted tile in world space.
    /// </summary>
    public Vector2 TileLocation
    {
        get { return highlightSprite.transform.position; }
        set { highlightSprite.transform.position = value; }
    }
    #endregion
    #region Initialization
    private void Awake()
    {
        Cursor.visible = false;
        cursorActiveProperty = Animator.StringToHash("CursorActive");
    }
    #endregion
    #region State Change Listeners
    private void OnLocationChanged(Vector2 worldLocation)
    {
        // Update cursor location.
        Location = worldLocation;
        // Update the highlighted tile location.
        // Running through these two functions clamps
        // to the nearest tile center.
        TileLocation = drivingController.Grid.GridToWorld(
            drivingController.Grid.WorldToGrid(worldLocation));
    }
    private void OnRenderStateChanged(RenderedCursorState newState)
    {
        RenderState = newState;
    }
    #endregion
}
