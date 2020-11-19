using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AgentCommander : Commander
{
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