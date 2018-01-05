using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteScroll : IgniteGUIElementGroup
    {
        float? scrollHeight;

        [SerializeField] Image background;
        [SerializeField] Image scrollbar;
        [SerializeField] Image handle;
        [SerializeField] Transform content;

        protected override Transform Content { get { return content; } }

        public override void SetSize(IIgniteGUISize size)
        {
            if (scrollHeight != null)
            {
                RectTransform.SetSizeDelta(y: scrollHeight.Value);
            }
        }

        public override void SetTheme(IIgniteGUITheme theme)
        {
            background.color = theme.ScrollBackground;
            scrollbar.color = theme.Scrollbar;
            handle.color = theme.ScrollHandle;
        }

        public override IObservable<Unit> OnSelected()
        {
            return Observable.Merge(
                background.OnPointerClickAsObservable().AsUnitObservable(),
                scrollbar.OnPointerClickAsObservable().AsUnitObservable(),
                handle.OnPointerClickAsObservable().AsUnitObservable());
        }

        public static IgniteScroll Create(IIgniteGUIGroup parent, IgniteWindow window, float? scrollHeight = null, string id = "")
        {
            var scroll = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Scroll")).GetComponent<IgniteScroll>();
            scroll.Parent = parent;
            scroll.Window = window;
            scroll.scrollHeight = scrollHeight;
            scroll.ID = id;
            return scroll;
        }
    }

    public static class IgniteScrollExtensions
    {
        public static IIgniteGUIGroup AddScroll(this IIgniteGUIGroup group, float? scrollHeight = null, string id = "")
        {
            return group.Add(IgniteScroll.Create(group, group.Window, scrollHeight, id));
        }
    }
}