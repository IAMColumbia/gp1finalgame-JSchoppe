using System;
using UnityEngine;

public sealed class PhaseManager : MonoBehaviour
{
    [SerializeField] private Phase[] mapPhases = null;

    public event Action CycleExited;

    private int phase;

    public void StartCycle(TileGrid grid)
    {
        foreach (Phase phase in mapPhases)
            phase.grid = grid;
        phase = 0;
        mapPhases[0].Completed += OnPhaseComplete;
        mapPhases[0].Begin();
    }

    private void OnPhaseComplete()
    {
        mapPhases[phase].Completed -= OnPhaseComplete;
        phase++;
        if (phase > mapPhases.Length - 1)
            phase = 0;
        mapPhases[phase].Completed += OnPhaseComplete;
        mapPhases[phase].Begin();
    }
}
