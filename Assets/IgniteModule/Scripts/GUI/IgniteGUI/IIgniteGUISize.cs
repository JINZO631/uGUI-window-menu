using UnityEngine;

namespace IgniteModule.UI
{
    public interface IIgniteGUISize
    {
        Vector2 WindowSize { get; }
        RectOffset Padding { get; }

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

        // Image 
        float ImageHeight { get; }
    }
}