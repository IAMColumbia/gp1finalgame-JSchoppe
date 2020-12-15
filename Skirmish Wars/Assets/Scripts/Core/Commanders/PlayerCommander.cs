using System;
using UnityEngine;
using SkirmishWars.Unity;

public sealed class PlayerCommander : Commander
{
    // TODO pause functionality needs to be generalized
    // for multiple human players.
    private bool isPaused;

    public event Action<bool> PauseStateChanged;

    public PlayerCommander(byte teamID, TileGrid grid, CursorController controller, CommanderButtonsListener buttons)
        : base(teamID, grid, controller)
    {
        controller.SecondaryPressed += OnSecondaryPressed;
        controller.SecondaryReleased += OnSecondaryReleased;
        buttons.PausePressed += TogglePause;
        // TODO this is a very lazy way to quit the game.
        buttons.ExitPressed += () => { Application.Quit(); };
        IsSpying = false;
        isPaused = false;
    }

    private void OnSecondaryPressed(Vector2 location)
    {
        IsSpying = true;
    }
    private void OnSecondaryReleased(Vector2 location)
    {
        IsSpying = false;
    }

    protected override void OnClick(Vector2 location)
    {
        base.OnClick(location);
        // TODO this is a hotfix.
        // This state should not be managed here.
        if (targetedActor != null
            && targetedActor is CombatUnit unit)
            unit.HasFocus = true;
    }
    protected override void OnRelease(Vector2 location)
    {
        base.OnRelease(location);
        // TODO this is a hotfix.
        // This state should not be managed here.
        if (targetedActor != null
            && targetedActor is CombatUnit unit)
            unit.HasFocus = false;
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
        isPaused = !isPaused;
        PauseStateChanged?.Invoke(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0f;
            foreach (Commander commander in grid.Commanders)
                foreach (CombatUnit unit in commander.units)
                    unit.PathShown = false;
        }
        else
        {
            Time.timeScale = 1f;
            foreach (CombatUnit unit in units)
                unit.PathShown = true;
        }
    }
}