using System;

[Serializable]
public struct DamageTableEntry
{
    public UnitType attackingUnit;
    public UnitType defendingUnit;
    public float baseDamage;
}
