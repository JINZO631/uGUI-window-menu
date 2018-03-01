using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteHorizontalGroup : IgniteGUIElementGroup
    {
        [SerializeField] Transform content;
        [SerializeField] HorizontalLayoutGroup layoutGroup;

        public override Transform Content { get { return content; } }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            RectTransform.SetSizeDelta(y: size.ElementHeight);
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return Observable.Never<Unit>();
        }

        /// <summary> レイアウトの整列 </summary>
        public void SetLayout()
        {
            layoutGroup.SetLayoutHorizontal();
        }

        /// <summary> 生成 </summary>
        public static IgniteHorizontalGroup Create(string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/HorizontalGroup")).GetComponent<IgniteHorizontalGroup>();

            instance.ID = id;  // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
                .SubscribeWithState(instance, (_, i) => i.SetLayout());

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        /// <summary> 横方向グループ追加 </summary>
        public static IIgniteGUIGroup AddHorizontalGroup(this IIgniteGUIGroup group, IIgniteGUIElement[] elements = null, string id = "")
        {
            var horizontal = IgniteHorizontalGroup.Create(id);

            if (elements != null)
            {
                foreach (var e in elements)
                {
                    horizontal.Add(e);
                }
            }

            return group.Add(horizontal);
        }
    }
}