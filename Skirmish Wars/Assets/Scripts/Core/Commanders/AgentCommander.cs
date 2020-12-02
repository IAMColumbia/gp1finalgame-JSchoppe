using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public sealed class AgentCommander : Commander
{
    public AgentCommander(byte teamID, TileGrid grid, CursorController controller)
        : base(teamID, grid, controller)
    {

    }


    public float strategizeFrequency;
    public float strategizeError;

    private void Start()
    {
        
    }
    protected override void OnCommandPhaseBegin()
    {
        
    }
    protected override void OnCommandPhaseEnd()
    {

    }


    private IEnumerator StrategizeLoop()
    {
        throw new NotImplementedException();
    }

    private void Strategize()
    {

    }
}