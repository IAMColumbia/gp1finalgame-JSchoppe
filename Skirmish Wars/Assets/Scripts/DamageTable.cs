using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class DamageTable
{
    public List<DamageTableEntry> designerData;

    public float this[UnitType firstUnit, UnitType secondUnit]
    {
        get { throw new NotImplementedException(); }
    }

    public float[,] baseDamageVals;
}