using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A timer renderer implemented in unity.
/// </summary>
public sealed class TimerRenderer : MonoBehaviour
{
    #region Private Fields
    private string secondsFormat;
    private HSVColor filledColor;
    private HSVColor emptyColor;
    private Coroutine clockTickRoutine;
    private Timer drivingTimer;
    #endregion
    #region Inspector Fields
    [Header("Component References")]
    [Tooltip("The radial fill sprite that shows clock interpolant.")]
    [SerializeField] private Image radialFillImage = null;
    [Tooltip("The text that prints the remaining seconds.")]
    [SerializeField] private Text clockText = null;
    [Header("Visual Settings")]
    [Range(0, 4)][Tooltip("How many decimal places to display on the clock.")]
    [SerializeField] private int decimalPlaces = 0;
    [Tooltip("Color of the meter when the most time is remaining.")]
    [SerializeField] private Color filledAlbedo = Color.green;
    [Tooltip("Color of the meter when the least time is remaining.")]
    [SerializeField] private Color emptyAlbedo = Color.red;
    // Allow the inspector to update decimal places during runtime:
    private void OnValidate() { ProcessInspectorFields(); }
    #endregion
    #region Properties
    /// <summary>
    /// The timer that drives this renderer.
    /// </summary>
    public Timer DrivingTimer
    {
        get { return drivingTimer; }
        set
        {
            // Detach from previous timer.
            if (clockTickRoutine != null)
                StopCoroutine(clockTickRoutine);
            if (drivingTimer != null)
            {
                drivingTimer.Started -= OnTimerStarted;
                drivingTimer.Elapsed -= OnTimerElapsed;
            }
            // Attach to new timer.
            value.Started += OnTimerStarted;
            value.Elapsed += OnTimerElapsed;
            if (value.IsRunning)
                OnTimerStarted();
            drivingTimer = value;
        }
    }
    #endregion
    #region Initialization (Start)
    private void Awake() { ProcessInspectorFields(); }
    private void ProcessInspectorFields()
    {
        // Convert designer RBG colors to HSV.
        filledColor = new HSVColor(filledAlbedo);
        emptyColor = new HSVColor(emptyAlbedo);
        // Precalculate the required format string.
        if (decimalPlaces == 0)
            secondsFormat = "{0:0}";
        else
        {
            string decimalSubstring = string.Empty;
            for (int i = 0; i < decimalPlaces; i++)
                decimalSubstring += "0";
            secondsFormat = "{0:0." + decimalSubstring + "}";
        }
    }
    #endregion
    #region UI Update Loop
    private void OnTimerStarted()
    {
        clockTickRoutine = StartCoroutine(WhileTimerTicking());
    }
    private IEnumerator WhileTimerTicking()
    {
        // Update the interface while the timer is going.
        while (true)
        {
            clockText.text = string.Format(secondsFormat, drivingTimer.RemainingSeconds);
            radialFillImage.fillAmount = 1 - drivingTimer.Interpolant;
            radialFillImage.color = HSVColor.Lerp(filledColor, emptyColor, drivingTimer.Interpolant).AsRGB;
            yield return null;
        }
    }
    private void OnTimerElapsed()
    {
        StopCoroutine(clockTickRoutine);
    }
    #endregion
}
