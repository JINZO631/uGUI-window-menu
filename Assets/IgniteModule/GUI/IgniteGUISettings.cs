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

            instance.font = Resources.Load<Font>("IgniteGUI/Fonts/SourceHanCodeJP-Normal");
            instance.fontSize = 14;
            instance.defaultWindowSize = new Vector2(400f, 600f);
            instance.elementSpacing = 5f;
            instance.elementHeight = 20;
            instance.elementWidth = 300;
            instance.fontColor = new Color32(224, 234, 247, 255);
            instance.windowHeaderColor = new Color32(12, 22, 33, 255);
            instance.windowContentColor = new Color32(12, 22, 33, 128);
            instance.windowDragAreaColor = new Color32(12, 22, 33, 255);
            instance.labelHighlightColor = new Color(1, 1, 1, 0.5f);
            instance.buttonColor = new Color32(73, 79, 88, 255);
            instance.sliderBackgroundColor = new Color32(9, 73, 151, 255);
            instance.sliderHandleColor = new Color32(103, 109, 118, 128);
            instance.toggleEnableColor = new Color32(13, 191, 102, 255);
            instance.toggleBackgroundColor = new Color32(73, 79, 88, 255);
            instance.foldoutColor = new Color32(9, 73, 151, 255);
            instance.dropdownColor = new Color32(9, 73, 151, 255);
            instance.inputFieldColor = new Color32(9, 73, 151, 255);

            Debug.Log(Application.dataPath + "/Resources");
            if (!System.IO.Directory.Exists(Application.dataPath + "/Resources/IgniteGUI"))
            {
                System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources/IgniteGUI");
            }

            UnityEditor.AssetDatabase.CreateAsset(instance, "Assets/Resources/IgniteGUI/IgniteGUISettings.asset");
            UnityEditor.Selection.activeObject = instance;
        }
#endif
    }
}