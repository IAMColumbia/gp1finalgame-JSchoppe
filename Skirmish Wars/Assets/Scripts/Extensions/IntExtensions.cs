using UnityEngine;

/// <summary>
/// Provides extension methods for the integer data type.
/// </summary>
public static class IntExtensions
{
    /// <summary>
    /// Clamps an integer value in place.
    /// </summary>
    /// <param name="value">The int to clamp.</param>
    /// <param name="min">The min clamp range.</param>
    /// <param name="max">The max clamp range.</param>
    public static void Clamp(this ref int value, int min, int max)
    {
        value = Mathf.Clamp(value, min, max);
    }
}
