using System;
using UnityEngine;
using SkirmishWars.UnityRenderers;

public sealed class CommandPhase : Phase
{
    [SerializeField] private float baseCommandTime = 30f;
    [SerializeField] private float timeAddedPerUnit = 0.5f;
    [SerializeField] private TimerRenderer timerRenderer = null;

    private Timer phaseTimer;

    public override event Action Completed;

    private void Awake()
    {
        phaseTimer = new Timer(baseCommandTime);
        if (timerRenderer != null)
            timerRenderer.DrivingTimer = phaseTimer;
        phaseTimer.Elapsed += OnTimeElapsed;
    }

    public override void Begin()
    {
        // Enable commander activity.
        foreach (Commander commander in grid.Commanders)
        {
            commander.controller.IsEnabled = true;
            commander.OnCommandPhaseBegin();
        }

        phaseTimer.Duration = baseCommandTime
            + timeAddedPerUnit * grid.Actors.Count;
        phaseTimer.Begin();
    }

    private void OnTimeElapsed()
    {
        // Disable commander activity.
        foreach (Commander commander in grid.Commanders)
        {
            commander.controller.IsEnabled = false;
            commander.OnCommandPhaseEnd();
        }
        // Start next phase.
        Completed?.Invoke();
    }
}
