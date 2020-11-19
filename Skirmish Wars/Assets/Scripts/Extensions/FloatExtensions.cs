using UnityEngine;

/// <summary>
/// Provides extension methods for the float data type.
/// </summary>
public static class FloatExtensions
{
    /// <summary>
    /// Clamps a floating point value in place.
    /// </summary>
    /// <param name="value">The float to clamp.</param>
    /// <param name="min">The min clamp range.</param>
    /// <param name="max">The max clamp range.</param>
    public static void Clamp(this ref float value, float min, float max)
    {
        value = Mathf.Clamp(value, min, max);
    }
}
