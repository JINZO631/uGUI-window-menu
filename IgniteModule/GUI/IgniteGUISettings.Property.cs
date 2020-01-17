using UnityEngine;

namespace IgniteModule.GUICore
{
    public partial class IgniteGUISettings
    {
        static IgniteGUISettings instance;
        public static IgniteGUISettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<IgniteGUISettings>("IgniteGUI/IgniteGUISettings");
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public static Font Font => Instance.font;
        public static int FontSize => Instance.fontSize;
        public static Vector2 DefaultWindowSize => Instance.defaultWindowSize;
        public static float ElementSpacing => Instance.elementSpacing;
        public static float ElementHeight => Instance.elementHeight;
        public static float ElementWidth => Instance.elementWidth;

        public static Color FontColor => Instance.fontColor;
        public static Color WindowHeaderColor => Instance.windowHeaderColor;
        public static Color WindowContentColor => Instance.windowContentColor;
        public static Color WindowDragAreaColor => Instance.windowDragAreaColor;
        public static Color LabelHighlightColor => Instance.labelHighlightColor;
        public static Color ButtonColor => Instance.buttonColor;
        public static Color SliderBackgroundColor => Instance.sliderBackgroundColor;
        public static Color SliderHandleColor => Instance.sliderHandleColor;
        public static Color ToggleEnableColor => Instance.toggleEnableColor;
        public static Color ToggleBackgroundColor => Instance.toggleBackgroundColor;
        public static Color FoldoutColor => Instance.foldoutColor;
        public static Color DropdownColor => Instance.dropdownColor;
        public static Color InputFieldColor => Instance.inputFieldColor;
    }
}