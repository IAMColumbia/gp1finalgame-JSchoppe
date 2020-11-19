using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class CommandPhase : Phase
{
    public float baseCommandTime;
    public float timeAddedPerUnit;

    public Timer phaseTimer;

    public override event Action Completed;

    public override void Begin()
    {
        throw new NotImplementedException();
    }
}