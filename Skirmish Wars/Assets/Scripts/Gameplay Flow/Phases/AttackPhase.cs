using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class AttackPhase : Phase
{
    public float unitMoveSpeed;
    public float confrontationPauseSeconds;
    public DamageTable damageTable;

    public override event Action Completed;

    public override void Begin()
    {
        throw new NotImplementedException();
    }

    private IEnumerator AnimateUnits()
    {
        throw new NotImplementedException();
    }

    private void CheckConfrontation()
    {

    }

    private IEnumerator AnimateConfrontation()
    {
        throw new NotImplementedException();
    }
}