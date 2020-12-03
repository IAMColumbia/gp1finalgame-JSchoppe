using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class AgentCommander : Commander
{
    public AgentCommander(byte teamID, TileGrid grid, AgentCursorController controller)
        : base(teamID, grid, controller)
    {
        agentCursor = controller;
        // TODO expose these to the inspector.
        spyTimer = new Timer(0.4f);
        restrategizeTimer = new Timer(5f);
    }

    private AgentCursorController agentCursor;

    private Timer restrategizeTimer;
    private Timer spyTimer;

    public override void OnCommandPhaseBegin()
    {
        StrategizePause();
        restrategizeTimer.Elapsed += StrategizePause;
    }
    public override void OnCommandPhaseEnd()
    {
        restrategizeTimer.Elapsed -= StrategizePause;
        spyTimer.Elapsed -= StrategizeComplete;
        controller.RenderState = RenderedCursorState.Ghost;
    }

    private void StrategizePause()
    {
        controller.RenderState = RenderedCursorState.Ghost;
        spyTimer.Elapsed += StrategizeComplete;
        spyTimer.Begin();
    }
    private void StrategizeComplete()
    {
        // TODO this is a hotfix that only works
        // when there is exactly one other commander present.
        Commander targetCommander = null;
        foreach (Commander commander in grid.Commanders)
            if (commander.teamID != teamID)
                targetCommander = commander;

        // TODO make this movement behaviour better.
        agentCursor.ClearActions();
        if (targetCommander != null)
        {
            foreach (CombatUnit unit in units)
            {
                CombatUnit targetUnit = targetCommander.units.RandomElement();
                if (grid.TryFindPath(unit.Location, targetUnit.Location, unit.movement, unit.moveRange,
                    out Vector2Int[] path))
                {
                    Vector2Int[] fullPath = new Vector2Int[path.Length + 1];
                    fullPath[0] = unit.Location;
                    for (int i = 0; i < path.Length; i++)
                        fullPath[i + 1] = path[i];

                    if (path.Length > 1)
                    {
                        agentCursor.AddAction(
                            new CursorAction
                            {
                                path = grid.GridToWorld(fullPath),
                                holdsClick = true
                            },
                            OrderPriority.Queued
                        );
                    }
                }
            }
        }

        controller.RenderState = RenderedCursorState.Active;
        restrategizeTimer.Begin();
    }
}