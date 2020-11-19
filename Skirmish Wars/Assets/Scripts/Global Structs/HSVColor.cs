using UnityEngine;

/// <summary>
/// Color represented in HSV space.
/// </summary>
public struct HSVColor
{
    #region Fields
    public float hue;
    public float saturation;
    public float value;
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new HSV from a given RGB color.
    /// </summary>
    /// <param name="rgbColor">The original rgb color.</param>
    public HSVColor(Color rgbColor)
    {
        Color.RGBToHSV(rgbColor, out hue, out saturation, out value);
    }
    #endregion
    #region Properties
    /// <summary>
    /// Converts an HSV color back to the Color type.
    /// </summary>
    public Color AsRGB
    {
        get { return Color.HSVToRGB(hue, saturation, value); }
    }
    #endregion
    #region Methods
    /// <summary>
    /// Performs a lerp operation on a pair of HSV colors.
    /// </summary>
    /// <param name="left">The color at the 0 end of the range.</param>
    /// <param name="right">The color at the 1 end of the range.</param>
    /// <param name="interpolant">The interpolant between the colors.</param>
    /// <returns></returns>
    public static HSVColor Lerp(HSVColor left, HSVColor right, float interpolant)
    {
        return new HSVColor
        {
            hue = Mathf.Lerp(left.hue, right.hue, interpolant),
            saturation = Mathf.Lerp(left.saturation, right.saturation, interpolant),
            value = Mathf.Lerp(left.value, right.value, interpolant),
        };
    }
    #endregion
}
