using UnityEngine;
using System;

namespace IgniteModule.GUICore
{
    public static class LuminanceUtility
    {
        public static float ToLuminance(Color color)
        {
            return color.r * 0.3f + color.g * 0.59f + color.b * 0.11f;
        }

        public static Color ChooseFontColor(Color backgroundColor)
        {
            var backgroundLuminance = ToLuminance(backgroundColor);
            var whiteContrast = Mathf.Max(1, backgroundLuminance) / Mathf.Min(1, backgroundLuminance);
            var blackContrast = Mathf.Max(0, backgroundLuminance) / Mathf.Min(0, backgroundLuminance);

            return whiteContrast < blackContrast ? Color.black : Color.white;
        }

    }
}