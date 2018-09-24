using UnityEngine;

namespace IgniteModule.GUICore
{
    public static class IgniteGUISettings
    {
        public static Font Font { get; } = Resources.Load<Font>("IgniteGUI/Fonts/SourceHanCodeJP-Normal");
        public static int FontSize { get; } = 14;
        public static Vector2 DefaultWindowSize { get; } = new Vector2(400f, 600f);
        public static float ElementHeight { get; } = 20f;
        public static float ElementWidth { get; } = 100f;

        public static Color FontColor { get; } = new Color32(224, 234, 247, 255);
        public static Color WindowHeaderColor { get; } = new Color32(12, 22, 33, 255);
        public static Color WindowContentColor { get; } = new Color32(12, 22, 33, 128);
        public static Color WindowDragAreaColor { get; } = new Color32(12, 22, 33, 255);
        public static Color LabelHighlightColor { get; } = new Color(1, 1, 1, 0.5f);
        public static Color ButtonColor { get; } = new Color32(73, 79, 88, 255);
        public static Color SliderBackgroundColor { get; } = new Color32(9, 73, 151, 255);
        public static Color SliderHandleColor { get; } = new Color32(73, 79, 88, 128);
        public static Color ToggleEnableColor { get; } = new Color32(13, 191, 102, 255);
        public static Color ToggleBackgroundColor { get; } = new Color32(73, 79, 88, 255);
        public static Color FoldoutColor { get; } = new Color32(9, 73, 151, 255);
        public static Color DropdownColor { get; } = new Color32(9, 73, 151, 255);
        public static Color InputFieldColor { get; } = new Color32(9, 73, 151, 255);
    }
}