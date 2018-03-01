using UnityEngine;

namespace IgniteModule.UI
{
    [CreateAssetMenu(fileName = "IgniteGUISize", menuName = "IgniteGUI/Size")]
    public class IgniteGUISize : ScriptableObject, IIgniteGUISize
    {
        static IgniteGUISize defaultInstance;
        public static IgniteGUISize Default { get { return defaultInstance != null ? defaultInstance : (defaultInstance = Resources.Load<IgniteGUISize>("IgniteGUI/IgniteGUISize")); } }

        [SerializeField] Vector2 windowSize;
        public Vector2 WindowSize 
        {
            get { return windowSize; }
            set { windowSize = value; }
        }

        [SerializeField] RectOffset padding;
        public RectOffset Padding 
        {
            get { return padding; }
            set { padding = value;}
        }

        [SerializeField] float fontSize;
        public float FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        [SerializeField] float elementHeight;
        public float ElementHeight 
        {
            get { return elementHeight; }
            set { elementHeight = value; }
        }

        [SerializeField] Vector2 toggleSize;
        public Vector2 ToggleSize 
        {
            get { return toggleSize; }
            set { toggleSize = value; }
        }

        [SerializeField] float sliderWidth;
        public float SliderWidth
        {
            get { return sliderWidth; }
            set { sliderWidth = value; }
        }

        [SerializeField] float scrollbarHeight;
        public float ScrollbarHeight 
        {
            get { return scrollbarHeight; }
            set { scrollbarHeight = value; }
        }

        [SerializeField] float inputFieldWidth;
        public float InputFieldWidth
        { 
            get { return inputFieldWidth; }
            set { inputFieldWidth = value; }
        }

        [SerializeField] float imageHeight;
        public float ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; }
        }
    }
}