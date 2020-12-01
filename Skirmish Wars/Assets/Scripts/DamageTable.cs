using System;
using System.Collections.Generic;

// TODO make this enum-generic. Looks doable from
// a quick google search but kind of jank. Don't
// need it right now.

/// <summary>
/// Damage table for unit types.
/// </summary>
public sealed class DamageTable
{
    #region Immutable Fields
    private readonly float[,] baseDamage;
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new damage table using a collection of entries.
    /// </summary>
    /// <param name="entries">The entries for unit base damage exchanges.</param>
    public DamageTable(IList<DamageTableEntry> entries)
    {
        // Creates a new intersection matrix with dimensions
        // of the unit type length.
        // (this assumes the UnitType enum does not skip order)
        byte[] unitTypes = (byte[])Enum.GetValues(typeof(UnitType));
        baseDamage = new float[unitTypes.Length, unitTypes.Length];
        // Populate the intersection with the given entries.
        foreach (DamageTableEntry entry in entries)
        {
            // If there are duplicate non-zero entries, throw
            // an exception.
            if (this[entry.attackingUnit, entry.defendingUnit] != 0f)
                throw new ArgumentException(
                    $"Multiple table entries for {entry.attackingUnit} attacking {entry.defendingUnit}");
            else
                baseDamage[(byte)entry.attackingUnit, (byte)entry.defendingUnit]
                    = entry.baseDamage;
        }
    }
    #endregion
    #region Accessor
    /// <summary>
    /// Retrieves the base damage for a unit interaction.
    /// </summary>
    /// <param name="attackingUnit">The unit type of the attacking unit.</param>
    /// <param name="defendingUnit">The unit type of the defending unit.</param>
    /// <returns></returns>
    public float this[UnitType attackingUnit, UnitType defendingUnit]
    {
        get
        {
            return baseDamage[(byte)attackingUnit, (byte)defendingUnit];
        }
    }
    #endregion
}
