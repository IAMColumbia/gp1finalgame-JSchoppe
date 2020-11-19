using System;
using UnityEngine;

/// <summary>
/// Represents a timer driven by the unity update loop.
/// </summary>
public sealed class Timer
{
    #region Private Fields
    private bool isRunning;
    private float startTime;
    private float duration;
    private float lastPauseTime;
    private float accumulatedPauseTime;
    #endregion
    #region Constructor + Destructor
    /// <summary>
    /// Creates a new timer with the given duration.
    /// </summary>
    /// <param name="duration">Timer duration in seconds.</param>
    public Timer(float duration)
    {
        Duration = duration;
        isRunning = false;
    }
    /// <summary>
    /// Creates a new timer with the default duration of one second.
    /// </summary>
    public Timer() : this(1f) { }
    ~Timer()
    {
        // TODO research whether this actually helps
        // garbage collection.
        UpdateContext.Update -= Update;
    }
    #endregion
    #region Events
    /// <summary>
    /// Called when this timer is started or restarted.
    /// </summary>
    public event Action Started;
    /// <summary>
    /// Called when this timer elapses.
    /// </summary>
    public event Action Elapsed;
    #endregion
    #region Properties
    /// <summary>
    /// The current interpolant of timer completion.
    /// </summary>
    public float Interpolant { get; private set; }
    /// <summary>
    /// The current remaining seconds on the timer.
    /// </summary>
    public float RemainingSeconds { get; private set; }
    /// <summary>
    /// The duration of the timer. Is always greater than 0.
    /// </summary>
    public float Duration
    {
        get { return duration; }
        set
        {
            if (value <= 0f)
                throw new ArgumentOutOfRangeException("Timer duration must be greater than 0.");
            duration = value;
        }
    }
    #endregion
    #region Public Timer Methods
    /// <summary>
    /// Starts the timer, or restarts the timer if already started.
    /// </summary>
    public void Begin()
    {
        startTime = Time.time;
        accumulatedPauseTime = 0f;
        if (!isRunning)
        {
            isRunning = true;
            UpdateContext.Update += Update;
        }
    }
    /// <summary>
    /// Pauses the timer if it is currently running.
    /// </summary>
    public void Pause()
    {
        if (isRunning)
        {
            lastPauseTime = Time.time;
            UpdateContext.Update -= Update;
        }
    }
    /// <summary>
    /// Resumes the timer assuming it is currently paused.
    /// </summary>
    public void Resume()
    {
        // TODO this may be a source of bugs if this method
        // is called multiple times and over-accumulates pause time.
        if (isRunning)
        {
            accumulatedPauseTime += Time.time - lastPauseTime;
            UpdateContext.Update += Update;
        }
    }
    #endregion
    #region Update Loop
    private void Update()
    {
        // Update the accessible properties.
        Interpolant = Mathf.Clamp01(
            (Time.time - accumulatedPauseTime - startTime) / duration
        );
        RemainingSeconds = duration * (1 - Interpolant);
        // If the timer has elapsed;
        // notify listeners and exit update loop.
        if (Interpolant == 1f)
        {
            Elapsed();
            UpdateContext.Update -= Update;
        }
    }
    #endregion
}
