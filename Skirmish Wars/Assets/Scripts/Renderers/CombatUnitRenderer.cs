using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CombatUnitRenderer : MonoBehaviour
{
    public SpriteRenderer unitSprite;

    public SpriteRenderer hitpointsSprite;

    public NumberSpriteSet numberSpriteSet;

    private CombatUnit drivingUnit;

    public CombatUnit DrivingUnit
    {
        set
        {
            if (drivingUnit != null)
            {
                drivingUnit.PathChanged -= OnPathChanged;
                drivingUnit.TeamChanged -= UpdateVisualProperties;
                drivingUnit.Teleported -= UpdatePosition;
                drivingUnit.MovementAnimating -= OnMovementAnimating;
                drivingUnit.HitPointsChanged -= OnHitPointsChanged;
            }
            value.PathChanged += OnPathChanged;
            value.TeamChanged += UpdateVisualProperties;
            value.Teleported += UpdatePosition;
            value.MovementAnimating += OnMovementAnimating;
            value.HitPointsChanged += OnHitPointsChanged;
            drivingUnit = value;
            OnHitPointsChanged(drivingUnit.hitPoints);
        }
    }

    private void OnHitPointsChanged(float newHitPoints)
    {
        if (newHitPoints < 0f)
        {
            drivingUnit.PathChanged -= OnPathChanged;
            drivingUnit.TeamChanged -= UpdateVisualProperties;
            drivingUnit.Teleported -= UpdatePosition;
            drivingUnit.MovementAnimating -= OnMovementAnimating;
            drivingUnit.HitPointsChanged -= OnHitPointsChanged;
            Destroy(gameObject);
        }

        int hpNumber = Mathf.CeilToInt(newHitPoints * 10f);
        if (hpNumber > 9)
            hitpointsSprite.enabled = false;
        else
        {
            hitpointsSprite.enabled = true;
            hitpointsSprite.sprite = numberSpriteSet[hpNumber];
        }
    }

    private void UpdateVisualProperties(Team team)
    {
        unitSprite.color = team.style.baseColor;
        unitSprite.flipX = team.style.flipX;
    }

    private void UpdatePosition(Vector2Int newLocation)
    {
        transform.position = drivingUnit.Grid.GridToWorld(newLocation);
    }

    private void OnPathChanged(Vector2Int[] newPath)
    {
        worldPath = drivingUnit.Grid.GridToWorld(newPath);
        movementChain.Chain = worldPath;
    }

    private Vector2[] worldPath;

    private void OnMovementAnimating(float interpolant)
    {
        unitSprite.transform.position = Vector2.Lerp(worldPath[0], worldPath[1], interpolant);
    }

    public SpriteChainRenderer movementChain;
}
