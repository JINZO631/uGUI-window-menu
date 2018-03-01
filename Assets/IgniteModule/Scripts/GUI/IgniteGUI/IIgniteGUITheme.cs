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
        Color Button { get; }
        ColorBlock ButtonTransition { get; }

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
        Color DropdownFont { get; }
        Color DropdownBackground { get; }
        Color DropdownOptionsFont { get; }
        Color DropdownOptionsBackground { get; }
        Color DropdownCheckmark { get; }

        // FoldOut
        Color FoldOutBackground { get; }

        // InputField
        Color InputFieldBackground { get; }
        Color InputFieldFont { get; }
        ColorBlock InputFieldTransition { get; }

        // Image 
        Color ImageBackground { get; }
    }
}