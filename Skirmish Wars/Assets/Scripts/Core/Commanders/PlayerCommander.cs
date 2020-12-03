using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class PlayerCommander : Commander
{
    public PlayerCommander(byte teamID, TileGrid grid, CursorController controller)
        : base(teamID, grid, controller)
    {
        controller.SecondaryPressed += OnSecondaryPressed;
        controller.SecondaryReleased += OnSecondaryReleased;
        IsSpying = false;
    }

    private void OnSecondaryPressed(Vector2 location)
    {
        IsSpying = true;
    }
    private void OnSecondaryReleased(Vector2 location)
    {
        IsSpying = false;
    }

    private bool IsSpying
    {
        set
        {
            foreach (Commander commander in grid.Commanders)
                if (commander != this)
                    foreach (CombatUnit unit in commander.units)
                        unit.PathShown = value;
            foreach (CombatUnit unit in units)
                unit.PathShown = !value;
        }
    }

    private void TogglePause()
    {

    }
}