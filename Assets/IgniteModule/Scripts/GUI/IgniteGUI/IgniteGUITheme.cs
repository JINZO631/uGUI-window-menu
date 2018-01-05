using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    [CreateAssetMenu(fileName = "IgniteGUITheme", menuName = "IgniteGUI/Theme")]
    public class IgniteGUITheme : ScriptableObject, IIgniteGUITheme
    {
        static IgniteGUITheme defaultInstance;
        public static IgniteGUITheme Default { get { return defaultInstance != null ? defaultInstance : (defaultInstance = Resources.Load<IgniteGUITheme>("IgniteGUI/IgniteGUITheme")); } }

        [SerializeField] Color font = Color.white;
        public Color Font { get { return font; } }

        [Header("Window")]
        [SerializeField] Color windowHeader;
        public Color WindowHeader { get { return windowHeader; } }

        [SerializeField] Color windowBackground;
        public Color WindowBackground { get { return windowBackground; } }

        [SerializeField] Color windowCloseButton;
        public Color WindowCloseButton { get { return windowCloseButton; } }

        [SerializeField] ColorBlock windowCloseButtonTransitionColor;
        public ColorBlock WindowCloseButtonTransitionColor { get { return windowCloseButtonTransitionColor; } }

        [Header("Label")]
        [SerializeField] Color labelHighlightedColor = new Color(1, 1, 1, 0.5f);
        public Color LabelHighlitedColor { get { return labelHighlightedColor; } }

        [Header("Button")]
        [SerializeField] Color buttonColor = Color.gray;
        public Color ButtonColor { get { return buttonColor; } }

        [SerializeField] ColorBlock buttonTransitionColor = ColorBlock.defaultColorBlock;
        public ColorBlock ButtonTransitionColor { get { return buttonTransitionColor; } }

        [Header("Toggle")]
        [SerializeField] Color toggleEnable;
        public Color ToggleEnable { get { return toggleEnable; } }

        [SerializeField] Color toggleDisable;
        public Color ToggleDisable { get { return toggleDisable; } }

        [Header("Slider")]
        [SerializeField] Color sliderBackbround;
        public Color SliderBackground { get { return sliderBackbround; } }

        [SerializeField] Color sliderHandle;
        public Color SliderHandle { get { return sliderHandle; } }

        [SerializeField] Color sliderText;
        public Color SliderText { get { return sliderText; } }

        [Header("Scroll")]
        [SerializeField] Color scrollBackGround;
        public Color ScrollBackground { get { return scrollBackGround; } }

        [SerializeField] Color scrollBar;
        public Color Scrollbar { get { return scrollBar; } }

        [SerializeField] Color scrollHandle;
        public Color ScrollHandle { get { return scrollHandle; } }

        [Header("Dropdown")]
        [SerializeField] Color dropdownColor;
        public Color DropdownColor { get { return dropdownColor; } }

        [SerializeField] Color dropdownBackground;
        public Color DropdownBackground { get { return dropdownBackground; } }

        [Header("FoldOut")]
        [SerializeField] Color foldOutBackground;
        public Color FoldOutBackground { get { return foldOutBackground; } }
    }
}