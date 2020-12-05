using System;
using System.Collections.Generic;

#region Exposed Structs
[Serializable]
/// <summary>
/// Holds data about how a unit can traverse this terrain.
/// </summary>
public struct UnitTerrainData
{
    #region Fields
    /// <summary>
    /// If false this unit cannot move onto this tile.
    /// </summary>
    public bool canTraverse;
    /// <summary>
    /// The move cost multiplier for this tile.
    /// </summary>
    public int moveCost;
    /// <summary>
    /// The defense gained by being on this tile.
    /// </summary>
    public float addedDefense;
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new unit terrain relationship.
    /// </summary>
    /// <param name="canTraverse">Whether this unit can traverse this tile.</param>
    /// <param name="moveCost">The move cost for this tile (1 by default).</param>
    /// <param name="addedDefense">The additional defense added by being on this tile.</param>
    public UnitTerrainData(bool canTraverse, int moveCost, float addedDefense)
    {
        this.canTraverse = canTraverse;
        this.moveCost = moveCost;
        this.addedDefense = addedDefense;
    }
    #endregion
}
#endregion

/// <summary>
/// Represents a type of terrain and can be queried
/// to give data on how it interacts with units.
/// </summary>
public sealed class TileTerrain
{
    #region Fields
    private readonly UnitTerrainData[] unitData;
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new tile terrain with the given unit interaction data.
    /// </summary>
    /// <param name="unitEntries">The terrain-unit interaction data.</param>
    public TileTerrain(Dictionary<UnitType, UnitTerrainData> unitEntries)
    {
        // Initialize the unit data. Any unfilled
        // fields will be left as default (non-traversable).
        unitData =
            new UnitTerrainData[Enum.GetValues(typeof(UnitType)).Length];
        // Fill in table entries provided by caller.
        foreach (UnitType unitType in unitEntries.Keys)
            unitData[(byte)unitType] = unitEntries[unitType];
    }
    #endregion
    #region Properties
    /// <summary>
    /// Retrieves data about how this terrain interacts with a unit type.
    /// </summary>
    /// <param name="unit">The unit type interacting with the terrain.</param>
    /// <returns>Traversal and defense data for this interaction.</returns>
    public UnitTerrainData this[UnitType unit]
    {
        get { return unitData[(byte)unit]; }
    }
    #endregion
}
