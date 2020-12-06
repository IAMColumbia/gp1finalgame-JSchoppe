using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// TODO NeuralCommander could be a cool way to explore NNs.

/// <summary>
/// A commander that is controlled by a programmed agent.
/// </summary>
public sealed class AgentCommander : Commander
{
    #region Reference Fields
    private readonly DamageTable damageTable;
    private readonly AgentCursorController agentCursor;
    private readonly Timer thoughtTimer;
    #endregion
    #region State Fields
    private List<CombatUnit> targetUnits;
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new agent commander with the given parameters.
    /// </summary>
    /// <param name="teamID">Identifies how this commander interacts with other units.</param>
    /// <param name="grid">The grid that this commander will be placed on.</param>
    /// <param name="damageTable">Drives the AI behaviour of this agent.</param>
    /// <param name="controller">The cursor controller that this agent drives.</param>
    public AgentCommander(byte teamID, TileGrid grid,
        DamageTable damageTable, AgentCursorController controller)
        : base(teamID, grid, controller)
    {
        this.damageTable = damageTable;
        agentCursor = controller;
        thoughtTimer = new Timer();
    }
    #endregion
    #region Properties
    /// <summary>
    /// The number of seconds that the agent thinks for before
    /// making moves.
    /// </summary>
    public float ThoughtTime
    {
        set { thoughtTimer.Duration = value; }
    }
    #endregion
    #region Command Phase Listeners
    public override void OnCommandPhaseBegin()
    {
        // Scans the grid to update the current targeted units
        // that the AI may want to attack.
        targetUnits = new List<CombatUnit>();
        foreach (Commander commander in grid.Commanders)
            if (commander.teamID != teamID)
                targetUnits.AddRange(commander.units);
        // Setup the thought-action loop for this phase.
        StrategizePause();
        thoughtTimer.Elapsed += StrategizeComplete;
        agentCursor.ActionsExhausted += StrategizePause;
    }
    public override void OnCommandPhaseEnd()
    {
        // Stop the thought-action loop for this phase.
        agentCursor.ActionsExhausted -= StrategizePause;
        thoughtTimer.Elapsed -= StrategizeComplete;
        controller.RenderState = RenderedCursorState.Ghost;
    }
    #endregion
    #region Agent Strategy
    private void StrategizePause()
    {
        // Simulate a thought interval for this AI.
        controller.RenderState = RenderedCursorState.Ghost;
        thoughtTimer.Begin();
    }
    private void StrategizeComplete()
    {
        // Unghost the agent cursor.
        controller.RenderState = RenderedCursorState.Active;
        // Choose movement actions
        // for each of the units.
        foreach (CombatUnit unit in units)
        {
            foreach (CombatUnit enemy in targetUnits)
            {
                // Is this an advantageous or at least equal
                // damage tradeoff based on unit type?
                if (damageTable[unit.type, enemy.type]
                    >= damageTable[enemy.type, unit.type])
                {
                    // Attempt to meet the opposing unit at
                    // the end of their path.
                    // TODO holy shit this ternary operator is mf ugly,
                    // please make this not stupid
                    Vector2Int target
                        = (enemy.MovePath.Last != null) ?
                        enemy.MovePath.Last.Value
                        : enemy.Location;
                    if (grid.TryFindPath(unit.Location, target, unit.type, unit.moveRange,
                        out Vector2Int[] path))
                    {
                        // Append the starting tile, this is needed
                        // since the cursor has to click on the unit.
                        Vector2Int[] fullPath = new Vector2Int[path.Length + 1];
                        fullPath[0] = unit.Location;
                        path.CopyTo(fullPath, 1);
                        // Add the cursor action to move this unit,
                        // if it is a valid path of greater than one length
                        // and if the identical action has not been taken.
                        if (path.Length > 1
                            && !unit.MovePath.SequenceEqual(fullPath))
                        {
                            agentCursor.AddAction(
                                new CursorAction
                                {
                                    path = grid.GridToWorld(fullPath),
                                    holdsClick = true
                                },
                                OrderPriority.Queued
                            );
                            // TODO clean this shit up for the love of
                            // god this break statement is so far away
                            // from the foreach block.
                            break;
                        }
                    }
                }
            }
        }
    }
    #endregion
}
