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

        public override Transform Content { get { return content; } }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            if (scrollHeight != null)
            {
                SetSize(scrollHeight.Value);
            }
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(float height)
        {
            RectTransform.SetSizeDelta(y: height);
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.ScrollBackground, theme.Scrollbar, theme.ScrollHandle);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? backgroundColor = null, Color? scrollbarColor = null, Color? handleColor = null)
        {
            if (backgroundColor.HasValue) background.color = backgroundColor.Value;
            if (scrollbarColor.HasValue) scrollbar.color = scrollbarColor.Value;
            if (handleColor.HasValue) handle.color = handleColor.Value;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return Observable.Merge(
                background.OnPointerClickAsObservable().AsUnitObservable(),
                scrollbar.OnPointerClickAsObservable().AsUnitObservable(),
                handle.OnPointerClickAsObservable().AsUnitObservable());
        }

        /// <summary> 生成 </summary>
        public static IgniteScroll Create(float? scrollHeight = null, Action<IgniteScroll> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Scroll")).GetComponent<IgniteScroll>();

            instance.scrollHeight = scrollHeight;  // 高さ設定
            instance.ID           = id;            // ID設定

            // 初期化時イベントが設定されていれば呼び出し
            if (onInitialize != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, onInitialize, (_, i, onInit) =>
                {
                    onInit(i);
                });
            }

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        /// <summary> スクロール追加 </summary>
        public static IIgniteGUIGroup AddScroll(this IIgniteGUIGroup group, float? scrollHeight = null, Action<IgniteScroll> onInitialize = null, string id = "")
        {
            return group.Add(IgniteScroll.Create(scrollHeight, onInitialize, id));
        }
    }
}