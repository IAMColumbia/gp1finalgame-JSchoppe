using System;
using System.Collections.Generic;
using UnityEngine; // Only used for common Vector2 type.

#region Exposed Enums/Classes
/// <summary>
/// Defines the order priority of an incoming item.
/// </summary>
public enum OrderPriority : byte
{
    Immediate, Queued
}
/// <summary>
/// Describes a cursor action in world space.
/// </summary>
public sealed class CursorAction
{
    public Vector2[] path;
    public bool holdsClick;
}
#endregion

/// <summary>
/// Cursor controller that follows agent commands.
/// </summary>
public sealed class AgentCursorController : CursorController
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
    #region State Fields
    private List<CursorAction> actions;
    private CursorAction currentAction;
    private bool isEnabled;
    private float speed;
    private int pathIndex;
    private ActionState actionState;
    private enum ActionState : byte
    {
        AwaitingAction,
        MovingTowardsStart,
        MovingTowardsEnd,
        DraggingTowardsEnd
    }
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new agent cursor controller on the given grid.
    /// </summary>
    /// <param name="grid">The tilegrid that this cursor will exist on.</param>
    /// <param name="speed">The speed of the cursor in tiles per second.</param>
    public AgentCursorController(TileGrid grid, float speed)
        : base(grid)
    {
        // Adjust speed to grid scale.
        this.speed = speed * grid.GridUnit;
        // Initialize actions.
        actions = new List<CursorAction>();
        actionState = ActionState.AwaitingAction;
    }
    #endregion
    #region Properties
    /// <summary>
    /// Controls whether this agent cursor will process in
    /// </summary>
    public override bool IsEnabled
    {
        get { return isEnabled; }
        set
        {
            isEnabled = value;
            if (!isEnabled)
            {
                // Exit interaction based on state.
                switch (actionState)
                {
                    case ActionState.MovingTowardsStart:
                        UpdateContext.Update -= OnUpdateToStart;
                        break;
                    case ActionState.MovingTowardsEnd:
                        UpdateContext.Update -= OnUpdateAlong;
                        break;
                    case ActionState.DraggingTowardsEnd:
                        UpdateContext.Update -= OnUpdateAlong;
                        // Break held pressed update cycle.
                        InterruptController();
                        // Release the cursor at its starting location.
                        PrimaryReleased?.Invoke(currentAction.path[0]);
                        break;
                }
                actions.Clear();
                actionState = ActionState.AwaitingAction;
            }
        }
    }
    #endregion
    #region Update Routines
    private void OnUpdateToStart()
    {
        // Move the cursor towards the cursor action start point.
        if (MoveCursorTowards(currentAction.path[0]))
        {
            if (currentAction.holdsClick)
            {
                // Simulate mouse click.
                PrimaryPressed?.Invoke(currentAction.path[0]);
                // Update state (for interruptor method).
                actionState = ActionState.DraggingTowardsEnd;
            }
            else
                actionState = ActionState.MovingTowardsEnd;
            // Change routine.
            UpdateContext.Update -= OnUpdateToStart;
            UpdateContext.Update += OnUpdateAlong;
        }
    }
    private void OnUpdateAlong()
    {
        // Move the cursor towards the next point on the path.
        if (MoveCursorTowards(currentAction.path[pathIndex]))
        {
            // Check if the end point has been reached.
            if (pathIndex == currentAction.path.Length - 1)
            {
                if (currentAction.holdsClick)
                    // Simulate mouse release.
                    PrimaryReleased?.Invoke(currentAction.path[pathIndex]);
                // Change routine.
                UpdateContext.Update -= OnUpdateAlong;
                PullAction();
            }
            else
                pathIndex++;
        }
    }
    // Returns true once the cursor eaches the location.
    private bool MoveCursorTowards(Vector2 targetLocation)
    {
        // Calculate movement delta.
        float deltaMovement = Time.deltaTime * speed;
        // Check if it oversteps the target.
        if (Vector2.Distance(WorldLocation, targetLocation) < deltaMovement)
        {
            WorldLocation = targetLocation;
            return true;
        }
        else
        {
            WorldLocation += (targetLocation - WorldLocation).normalized
                * deltaMovement;
            return false;
        }
    }
    private void PullAction()
    {
        // Check to see if the agent has queued any
        // additional actions.
        if (actions.Count > 0)
        {
            pathIndex = 1;
            actionState = ActionState.MovingTowardsStart;
            // Load and begin new action.
            currentAction = actions[0];
            actions.RemoveAt(0);
            UpdateContext.Update += OnUpdateToStart;
        }
        else
            actionState = ActionState.AwaitingAction;
    }
    #endregion
    #region Cursor Action Methods
    /// <summary>
    /// Removes all current actions that this agent cursor
    /// is performing. Will interrupt the current action.
    /// </summary>
    public void ClearActions()
    {
        // TODO this just toggles the enabled state.
        // Might need to be debugged to ensure all
        // neccasary state is flushed.
        IsEnabled = false;
        IsEnabled = true;
    }
    /// <summary>
    /// Adds a new action to the agent cursor controller.
    /// </summary>
    /// <param name="action">The cursor action to perform.</param>
    /// <param name="priority">When should this action be performed.</param>
    public void AddAction(CursorAction action, OrderPriority priority)
    {
        // Check for invalid action.
        if (action.path == null)
            throw new ArgumentNullException("action.path", "Path must have at least two points.");
        else if (action.path.Length < 2)
            throw new ArgumentException("Path must have at least two points.", "action.path");

        if (isEnabled)
        {
            // Add the action based on the action
            // priority.
            switch (priority)
            {
                case OrderPriority.Queued:
                    actions.Add(action);
                    break;
                case OrderPriority.Immediate:
                    actions.Insert(0, action);
                    break;
                default:
                    // Throw if order priority enum is updated,
                    // but the appropriate behavior has not been
                    // added for this switch block.
                    throw new NotImplementedException();
            }
            // If the controller is not performing an
            // action, start its Update routine.
            if (actionState == ActionState.AwaitingAction)
                PullAction();
        }
    }
    #endregion
}