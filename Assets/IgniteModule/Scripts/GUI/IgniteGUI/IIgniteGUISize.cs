using UnityEngine;

namespace IgniteModule.UI
{
    public interface IIgniteGUISize
    {
        Vector2 WindowSize { get; }
        RectOffset Padding { get; }
        float HeaderHeight { get; }
        float FontSize { get; }

        // LayoutElement
        float ElementHeight { get; }

        // Toggle
        Vector2 ToggleSize { get; }

        // Slider
        float SliderWidth { get; }

        // Scroll
        float ScrollbarHeight { get; }

        // InputField
        float InputFieldWidth { get; }
    }

    public class IgniteGUISizeDefault : IIgniteGUISize
    {
        public static IgniteGUISizeDefault Instance { get { return new IgniteGUISizeDefault(); } }

        public Vector2 WindowSize { get { return new Vector2(100, 200); } }

        public RectOffset Padding { get { return new RectOffset(5,5,5,5); } }
        public float HeaderHeight { get { return 15f; } }

        public float FontSize { get { return 11f; } }

        public float ElementHeight { get { return 15f; } }

        public Vector2 ToggleSize { get { return new Vector2(15f, 15f); } }

        public float SliderWidth { get { return 50f; } }

        public float ScrollbarHeight { get { return 10f; } }

        public float InputFieldWidth { get { return 200f; } }
    }
}