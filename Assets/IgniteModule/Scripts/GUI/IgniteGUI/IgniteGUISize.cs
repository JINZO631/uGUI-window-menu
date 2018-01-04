using UnityEngine;

namespace IgniteModule.UI
{
    [CreateAssetMenu(fileName = "IgniteGUISize", menuName = "IgniteGUI/Size")]
    public class IgniteGUISize : ScriptableObject, IIgniteGUISize
    {
        static IgniteGUISize defaultInstance;
        public static IgniteGUISize Default { get { return defaultInstance != null ? defaultInstance : (defaultInstance = Resources.Load<IgniteGUISize>("IgniteGUI/IgniteGUISize")); } }

        [SerializeField] Vector2 windowSize;
        public Vector2 WindowSize { get { return windowSize; } }

        [SerializeField] RectOffset padding;
        public RectOffset Padding { get { return padding; } }

        [SerializeField] float headerHeight;
        public float HeaderHeight { get { return headerHeight; } }

        [SerializeField] float fontSize;
        public float FontSize { get { return fontSize; } }

        [SerializeField] float elementHeight;
        public float ElementHeight { get { return elementHeight; } }

        [SerializeField] Vector2 toggleSize;
        public Vector2 ToggleSize { get { return toggleSize; } }

        [SerializeField] float sliderWidth;
        public float SliderWidth { get { return sliderWidth; } }

        [SerializeField] float scrollbarHeight;
        public float ScrollbarHeight { get { return scrollbarHeight; } }

        [SerializeField] float inputFieldWidth;
        public float InputFieldWidth { get { return inputFieldWidth; } }
    }
}