using System;
using System.Collections.Generic;
using System.Linq;

public sealed class Timer
{
    public float Duration { get; set; }

    public event Action Started;
    public event Action Elapsed;

    private float startTime;
    private float pauseTimeAccumulator;
    private float lastPauseTime;

    public float TimeRemaining { get; }


    public void Begin() { }
    public void Pause() { }
    public void Resume() { }
}