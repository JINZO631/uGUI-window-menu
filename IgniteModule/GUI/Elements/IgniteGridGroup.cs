using IgniteModule.GUICore;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule
{
    public class IgniteGridGroup : IgniteGUIElementGroup
    {
        [SerializeField] RectTransform content = null;
        [SerializeField] GridLayoutGroup layoutGroup = null;

        public override RectTransform Content => content;

        void Start()
        {
            SetLayout();
        }

        public void SetLayout()
        {
            layoutGroup.SetLayoutHorizontal();
        }

        public static IgniteGridGroup Create(Vector2 cellSize)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/GridGroup")).GetComponent<IgniteGridGroup>();

            instance.layoutGroup.cellSize = cellSize;
            instance.RectTransform.SetSizeDelta(y: IgniteGUISettings.ElementHeight);

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddGridGroup(this IIgniteGUIGroup group, Vector2 cellSize, params IIgniteGUIElement[] elements)
        {
            var grid = IgniteGridGroup.Create(cellSize);

            foreach (var e in elements)
            {
                grid.Add(e);
            }

            return group.Add(grid);
        }
    }
}