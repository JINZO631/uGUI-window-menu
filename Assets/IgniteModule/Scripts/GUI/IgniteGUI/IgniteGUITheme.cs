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
        public Color Font
        {
            get { return font; }
            set { font = value; }
        }

        [Header("Window")]
        [SerializeField] Color windowHeader;
        public Color WindowHeader
        {
            get { return windowHeader; }
            set { windowHeader = value; }
        }

        [SerializeField] Color windowBackground;
        public Color WindowBackground
        {
            get { return windowBackground; }
            set { windowBackground = value; }
        }

        [SerializeField] Color windowCloseButton;
        public Color WindowCloseButton
        {
            get { return windowCloseButton; }
            set { windowCloseButton = value; }
        }

        [SerializeField] ColorBlock windowCloseButtonTransitionColor = ColorBlock.defaultColorBlock;
        public ColorBlock WindowCloseButtonTransitionColor
        {
            get { return windowCloseButtonTransitionColor; }
            set { windowCloseButtonTransitionColor = value; }
        }

        [Header("Label")]
        [SerializeField] Color labelHighlightedColor = new Color(1, 1, 1, 0.5f);
        public Color LabelHighlitedColor
        {
            get { return labelHighlightedColor; }
            set { labelHighlightedColor = value; }
        }

        [Header("Button")]
        [SerializeField] Color button = Color.gray;
        public Color Button
        {
            get { return button; }
            set { button = value; }
        }

        [SerializeField] ColorBlock buttonTransition = ColorBlock.defaultColorBlock;
        public ColorBlock ButtonTransition
        {
            get { return buttonTransition; }
            set { buttonTransition = value; }
        }

        [Header("Toggle")]
        [SerializeField] Color toggleEnable;
        public Color ToggleEnable
        {
            get { return toggleEnable; }
            set { toggleEnable = value; }
        }

        [SerializeField] Color toggleDisable;
        public Color ToggleDisable
        {
            get { return toggleDisable; }
            set { toggleDisable = value; }
        }

        [Header("Slider")]
        [SerializeField] Color sliderBackbround;
        public Color SliderBackground
        {
            get { return sliderBackbround; }
            set { sliderBackbround = value; }
        }

        [SerializeField] Color sliderHandle;
        public Color SliderHandle
        {
            get { return sliderHandle; }
            set { sliderHandle = value; }
        }

        [SerializeField] Color sliderText;
        public Color SliderText
        {
            get { return sliderText; }
            set { sliderText = value; }
        }

        [Header("Scroll")]
        [SerializeField] Color scrollBackGround;
        public Color ScrollBackground
        {
            get { return scrollBackGround; }
            set { scrollBackGround = value; }
        }

        [SerializeField] Color scrollBar;
        public Color Scrollbar
        {
            get { return scrollBar; }
            set { scrollBar = value; }
        }

        [SerializeField] Color scrollHandle;
        public Color ScrollHandle
        {
            get { return scrollHandle; }
            set { scrollHandle = value; }
        }

        [Header("Dropdown")]
        [SerializeField] Color dropdownFont;
        public Color DropdownFont
        {
            get { return dropdownFont; }
            set { dropdownFont = value; }
        }

        [SerializeField] Color dropdownBackground;
        public Color DropdownBackground
        {
            get { return dropdownBackground; }
            set { dropdownBackground = value; }
        }

        [SerializeField] Color dropdownOptionsFont;
        public Color DropdownOptionsFont
        {
            get { return dropdownOptionsFont; }
            set { dropdownOptionsFont = value; }
        }

        [SerializeField] Color dropdownOptionsBackground;
        public Color DropdownOptionsBackground
        {
            get { return dropdownOptionsBackground; }
            set { dropdownOptionsBackground = value; }
        }

        [SerializeField] Color dropdownCheckmark;
        public Color DropdownCheckmark
        {
            get { return dropdownCheckmark; }
            set { dropdownCheckmark = value; }
        }

        [Header("FoldOut")]
        [SerializeField] Color foldOutBackground;
        public Color FoldOutBackground
        {
            get { return foldOutBackground; }
            set { foldOutBackground = value; }
        }

        [Header("InputField")]
        [SerializeField] Color inputFieldFont;
        public Color InputFieldFont
        {
            get { return inputFieldFont; }
            set { inputFieldFont = value; }
        }

        [SerializeField] Color inputFieldBackground;
        public Color InputFieldBackground
        {
            get { return inputFieldBackground; }
            set { inputFieldBackground = value; }
        }

        [SerializeField] ColorBlock inputFieldTransition = ColorBlock.defaultColorBlock;
        public ColorBlock InputFieldTransition
        {
            get { return inputFieldTransition; }
            set { inputFieldTransition = value; }
        }

        [Header("Image")]
        [SerializeField]
        Color imageBackground;
        public Color ImageBackground
        {
            get { return imageBackground; }
            set { imageBackground = value; }
        }
    }
}