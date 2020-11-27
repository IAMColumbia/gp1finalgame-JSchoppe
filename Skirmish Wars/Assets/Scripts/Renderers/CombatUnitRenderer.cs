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

    [SerializeField] private CombatUnit drivingUnit = null;

    private void Awake()
    {
        drivingUnit.PathChanged += OnPathChanged;
    }

    private void OnPathChanged(Vector2Int[] newPath)
    {
        movementChain.Chain =
            drivingUnit.Grid.GridToWorld(newPath);
    }

    public SpriteChainRenderer movementChain;
}
