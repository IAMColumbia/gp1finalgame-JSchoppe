using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO abstract away from MonoBehaviour.
// TODO this should arguably not be a phase
// because it is not easily generalized to
// other win conditions that may be able to
// terminate gameplay before turn end.

public sealed class CheckWinStatePhase : Phase
{
    public override event Action Completed;

    public override void Begin()
    {
        bool foundWinCondition = false;
        foreach (Commander commander in grid.Commanders)
        {
            if (commander.units.Count == 0)
            {
                // TODO make this better.
                SceneManager.LoadScene(0);
                foundWinCondition = true;
                break;
            }
        }
        if (!foundWinCondition)
            Completed?.Invoke();
    }
}
