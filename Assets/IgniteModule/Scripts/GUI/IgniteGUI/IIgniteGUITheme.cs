using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public interface IIgniteGUITheme
    {
        Color Font { get; }

        // Window
        Color WindowHeader { get; }
        Color WindowBackground { get; }
        Color WindowCloseButton { get; }
        ColorBlock WindowCloseButtonTransitionColor { get; }

        // Label
        Color LabelHighlitedColor { get; }

        // Button
        Color ButtonColor { get; }
        ColorBlock ButtonTransitionColor { get; }

        // Toggle
        Color ToggleEnable { get; }
        Color ToggleDisable { get; }

        // Slider
        Color SliderBackground { get; }
        Color SliderHandle { get; }
        Color SliderText { get; }

        // Scroll
        Color ScrollBackground { get;}
        Color Scrollbar { get; }
        Color ScrollHandle { get; }

        // Dropdown
        Color DropdownColor { get; }
        Color DropdownBackground { get; }

        // FoldOut
        Color FoldOutBackground { get; }
    }
}