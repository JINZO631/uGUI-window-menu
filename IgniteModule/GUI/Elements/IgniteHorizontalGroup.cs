using IgniteModule.GUICore;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule
{
    public class IgniteHorizontalGroup : IgniteGUIElementGroup
    {
        [SerializeField] RectTransform content = null;
        [SerializeField] HorizontalLayoutGroup layoutGroup = null;

        public override RectTransform Content => content;

        void Start()
        {
            SetLayout();
        }

        public void SetLayout()
        {
            layoutGroup.SetLayoutHorizontal();
        }

        public static IgniteHorizontalGroup Create()
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/HorizontalGroup")).GetComponent<IgniteHorizontalGroup>();

            instance.RectTransform.SetSizeDelta(y: IgniteGUISettings.ElementHeight);

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddHorizontalGroup(this IIgniteGUIGroup group, params IIgniteGUIElement[] elements)
        {
            var horizontal = IgniteHorizontalGroup.Create();

            foreach (var e in elements)
            {
                horizontal.Add(e);
            }

            return group.Add(horizontal);
        }
    }
}