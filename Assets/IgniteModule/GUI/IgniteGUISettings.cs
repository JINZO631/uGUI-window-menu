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
    }
}