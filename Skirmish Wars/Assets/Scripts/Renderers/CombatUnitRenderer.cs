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
            }
            value.PathChanged += OnPathChanged;
            value.TeamChanged += UpdateVisualProperties;
            value.Teleported += UpdatePosition;
            drivingUnit = value;
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
        movementChain.Chain =
            drivingUnit.Grid.GridToWorld(newPath);
    }

    public SpriteChainRenderer movementChain;
}
