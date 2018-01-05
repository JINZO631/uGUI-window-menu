using System;
using UniRx;
using UnityEngine;

namespace IgniteModule.UI
{
    public class IgniteHorizontalGroup : IgniteGUIElementGroup
    {
        [SerializeField] Transform content;

        protected override Transform Content { get { return content; } }

        public override void SetSize(IIgniteGUISize size)
        {
            RectTransform.SetSizeDelta(y: size.ElementHeight);
        }

        public override void SetTheme(IIgniteGUITheme theme)
        {
        }

        public override IObservable<Unit> OnSelected()
        {
            return Observable.Never<Unit>();
        }

        public static IgniteHorizontalGroup Create(IIgniteGUIGroup parent, string id = "")
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/HorizontalGroup")).GetComponent<IgniteHorizontalGroup>();
            instance.Parent = parent;
            instance.Window = parent.Window;
            instance.ID = id;
            return instance;
        }
    }

    public static class IgniteHorizontalGroupExtensions
    {
        public static IIgniteGUIGroup AddHorizontalGroup(this IIgniteGUIGroup group, IIgniteGUIElement[] elements, string id = "")
        {
            var horizontal = IgniteHorizontalGroup.Create(group, id);
            foreach (var e in elements)
            {
                horizontal.Add(e);
            }
            return group.Add(horizontal);
        }
    }
}