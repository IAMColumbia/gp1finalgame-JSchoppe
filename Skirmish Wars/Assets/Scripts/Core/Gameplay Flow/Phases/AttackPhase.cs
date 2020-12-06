using System;
using System.Collections.Generic;
using UnityEngine;

// TODO abstract away from MonoBehaviour.

/// <summary>
/// Gameplay phase where units move and confront each other.
/// </summary>
public sealed class AttackPhase : Phase
{
    [SerializeField] private float tilesPerSecond;
    [SerializeField] private float confrontationPauseSeconds;
    public DamageTable damageTable;

    public override event Action Completed;

    /// <summary>
    /// Called every frame during movement animation.
    /// Passes through the interpolant of the movement
    /// animation.
    /// </summary>
    public event Action<float> MovementAnimating;

    private void Awake()
    {
        // TODO remove this it is jank!
        // Should be passed via constructor.
        damageTable = new UnitySceneParser().GetFirstDamageTable();
    }

    private sealed class UnitAnimationState
    {
        public CombatUnit unit;
        public bool pathComplete;

        public UnitAnimationState(CombatUnit unit, bool pathComplete)
        {
            this.unit = unit;
            this.pathComplete = pathComplete;
        }
    }

    private List<UnitAnimationState> animationStates;

    private float interpolant;

    public override void Begin()
    {
        animationStates = new List<UnitAnimationState>();
        foreach (CombatUnit unit in grid.Actors)
            animationStates.Add(new UnitAnimationState(unit, unit.MovePath.Count <= 1));

        interpolant = 0f;
        UpdateContext.Update += AnimationFirstHalfUpdate;
    }

    private void AnimationFirstHalfUpdate()
    {
        interpolant += Time.deltaTime * tilesPerSecond;
        interpolant.Clamp(0f, 0.5f);

        foreach (UnitAnimationState state in animationStates)
            if (!state.pathComplete)
                state.unit.AnimateMovement(interpolant);
        if (interpolant == 0.5f)
        {
            UpdateContext.Update -= AnimationFirstHalfUpdate;
            CheckConfrontationMidpoint();
            UpdateContext.Update += AnimationSecondHalfUpdate;
        }
    }
    private void AnimationSecondHalfUpdate()
    {
        interpolant += Time.deltaTime * tilesPerSecond;
        interpolant.Clamp(0.5f, 1.0f);
        if (interpolant == 1.0f)
        {
            UpdateContext.Update -= AnimationSecondHalfUpdate;

            bool anyUnitsStillNeedToMove = false;
            foreach (UnitAnimationState state in animationStates)
            {
                if (!state.pathComplete)
                {
                    state.unit.AnimateMovement(0f);

                    LinkedList<Vector2Int> path = state.unit.MovePath;
                    path.RemoveFirst();
                    state.unit.Location = path.First.Value;
                    if (path.Count == 1)
                    {
                        state.pathComplete = true;
                        path.Clear();
                    }
                    else
                        anyUnitsStillNeedToMove = true;
                    state.unit.MovePath = path;
                }
            }
            CheckConfrontationEndpoint();
            if (anyUnitsStillNeedToMove)
            {
                interpolant = 0f;
                UpdateContext.Update += AnimationFirstHalfUpdate;
            }
            else
                Completed?.Invoke();
        }
        else
            foreach (UnitAnimationState state in animationStates)
                if (!state.pathComplete)
                    state.unit.AnimateMovement(interpolant);
    }


    private void CheckConfrontationMidpoint()
    {
        foreach (UnitAnimationState state in animationStates)
        {

        }
    }
    private void CheckConfrontationEndpoint()
    {
        Dictionary<Vector2Int, List<CombatUnit>> clusters
            = new Dictionary<Vector2Int, List<CombatUnit>>();

        foreach (UnitAnimationState state in animationStates)
        {
            if (clusters.ContainsKey(state.unit.Location))
                clusters[state.unit.Location].Add(state.unit);
            else
                clusters.Add(state.unit.Location, new List<CombatUnit>() { state.unit });
        }

        List<CombatUnit> removedUnits = new List<CombatUnit>();
        foreach (KeyValuePair<Vector2Int, List<CombatUnit>> cluster in clusters)
        {
            Dictionary<int, int> teamQuantities =
                new Dictionary<int, int>();
            Dictionary<CombatUnit, float> dealtDamage =
                new Dictionary<CombatUnit, float>();

            foreach (CombatUnit unit in cluster.Value)
            {
                dealtDamage.Add(unit, 0f);
                if (teamQuantities.ContainsKey(unit.TeamID))
                    teamQuantities[unit.TeamID]++;
                else
                    teamQuantities.Add(unit.TeamID, 1);
            }

            foreach (CombatUnit unit in cluster.Value)
            {
                float spreadFactor = 1f / (cluster.Value.Count - teamQuantities[unit.TeamID]);
                foreach (CombatUnit otherUnit in cluster.Value)
                    if (otherUnit != unit && otherUnit.TeamID != unit.TeamID)
                        dealtDamage[otherUnit] += spreadFactor * unit.hitPoints *
                            damageTable[unit.type, otherUnit.type];
            }

            foreach(CombatUnit unit in cluster.Value)
            {
                float defense =
                    grid.Terrain[cluster.Key][unit.type].addedDefense;
                unit.HitPoints -= dealtDamage[unit] * (1f - defense);
                if (unit.HitPoints < 0f)
                    removedUnits.Add(unit);
            }
        }

        foreach (CombatUnit removedUnit in removedUnits)
        {
            // TODO this is cringe and would not scale.
            // Make the accessor structure for commanders better.
            foreach (Commander commander in grid.Commanders)
                if (commander.teamID == removedUnit.TeamID)
                    commander.units.Remove(removedUnit);

            grid.Actors.Remove(removedUnit);
            for (int i = 0; i < animationStates.Count; i++)
            {
                if (animationStates[i].unit == removedUnit)
                {
                    animationStates.RemoveAt(i);
                    break;
                }
            }
        }
    }
}