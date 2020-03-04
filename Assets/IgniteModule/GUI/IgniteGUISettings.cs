using UnityEngine;

namespace IgniteModule.GUICore
{
    public partial class IgniteGUISettings : ScriptableObject
    {
        [SerializeField] Font font;
        [SerializeField] int fontSize;
        [SerializeField] Vector2 defaultWindowSize;
        [SerializeField] float elementSpacing;
        [SerializeField] float elementHeight;
        [SerializeField] float elementWidth;
        [SerializeField] Color fontColor;
        [SerializeField] Color windowHeaderColor;
        [SerializeField] Color windowContentColor;
        [SerializeField] Color windowDragAreaColor;
        [SerializeField] Color labelHighlightColor;
        [SerializeField] Color buttonColor;
        [SerializeField] Color sliderBackgroundColor;
        [SerializeField] Color sliderHandleColor;
        [SerializeField] Color toggleEnableColor;
        [SerializeField] Color toggleBackgroundColor;
        [SerializeField] Color foldoutColor;
        [SerializeField] Color dropdownColor;
        [SerializeField] Color inputFieldColor;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/IgniteGUI/CreateDefaultSettings")]
        public static void CreateDefaultSettings()
        {
            var instance = ScriptableObject.CreateInstance<IgniteGUISettings>();

            if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/IgniteGUI"))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/IgniteGUI");
            }

            UnityEditor.AssetDatabase.CreateAsset(instance, "Assets/Resources/IgniteGUI/IgniteGUISettings.asset");
            UnityEditor.Selection.activeObject = instance;
        }

        [ContextMenu("reset")]
        void Reset()
        {
            font = Resources.Load<Font>("IgniteGUI/Fonts/SourceHanCodeJP-Normal");
            fontSize = 14;
            defaultWindowSize = new Vector2(400f, 600f);
            elementSpacing = 5f;
            elementHeight = 20;
            elementWidth = 300;
            fontColor = new Color32(224, 234, 247, 255);
            windowHeaderColor = new Color32(12, 22, 33, 255);
            windowContentColor = new Color32(12, 22, 33, 128);
            windowDragAreaColor = new Color32(12, 22, 33, 255);
            labelHighlightColor = new Color(1, 1, 1, 0.5f);
            buttonColor = new Color32(73, 79, 88, 255);
            sliderBackgroundColor = new Color32(9, 73, 151, 255);
            sliderHandleColor = new Color32(103, 109, 118, 128);
            toggleEnableColor = new Color32(13, 191, 102, 255);
            toggleBackgroundColor = new Color32(73, 79, 88, 255);
            foldoutColor = new Color32(9, 73, 151, 255);
            dropdownColor = new Color32(9, 73, 151, 255);
            inputFieldColor = new Color32(9, 73, 151, 255);
        }

        [ContextMenu("for mobile")]
        void MobileSettings()
        {
            fontSize = 30;
            defaultWindowSize = new Vector2(800f, 800f);
            elementSpacing = 15f;
            elementHeight = 50;
            elementWidth = 300;
        }
#endif
    }
}