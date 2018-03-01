using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public sealed class IgniteToggleGroup : IgniteGUIElementGroup
    {
        [SerializeField] ToggleGroup toggleGroup;

        IDisposable onSelectedDisposable;
        Subject<Unit> onSelected = new Subject<Unit>();
        List<Toggle> toggles = new List<Toggle>();

        public ToggleGroup Group { get { return toggleGroup; } }
        public List<Toggle> Toggles { get { return toggles; } }
        public override Transform Content { get { return Transform.parent; } }

        /// <summary> 子の追加 </summary>
        public override IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            base.Add(element);

            OnInitializeAfterAsync().SubscribeWithState2(element, this, (_, e, i) => i.RegisterToggle(e));

            return this;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return onSelected;
        }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size) { }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme) { }

        /// <summary> トグル登録 </summary>
        void RegisterToggle(IIgniteGUIElement element)
        {
            // 追加されたようそからトグルを探す
            var addToggle = element as IgniteToggle;

            if (addToggle != null)
            {
                // 追加されたのがトグルならそのまま登録して終了
                RegisterToggle(addToggle);
                return;
            }

            // 追加要素がグループならさらにその子からトグルを探す
            var group = element as IIgniteGUIElementGroup;
            IgniteToggle[] addToggles = null;
            if (group != null)
            {
                addToggles = group.Content.GetComponentsInChildren<IgniteToggle>();
            }

            if (addToggles != null)
            {
                // 見つけたトグルをすべて登録
                foreach (var t in addToggles)
                {
                    RegisterToggle(t);
                }
            }
            else
            {
                // 追加要素にトグルは含まれていなかった
                Debug.LogWarning("IgniteToggleGroup: Toggle not found.");
            }
        }

        /// <summary> トグル登録 </summary>
        void RegisterToggle(IgniteToggle toggle)
        {
            toggles.Add(toggle.Toggle);
            if (onSelectedDisposable != null) onSelectedDisposable.Dispose();
            onSelectedDisposable = Observable.Merge(toggles.Select(t => t.OnValueChangedAsObservable().AsUnitObservable())).Multicast(onSelected).Connect();
            toggle.Toggle.group = toggleGroup;
        }

        /// <summary> 生成 </summary>
        public static IgniteToggleGroup Create(bool allowSwitchOff = false, Action<IgniteToggleGroup> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/ToggleGroup")).GetComponent<IgniteToggleGroup>();

            instance.ID = id;  // ID設定

            // 初期化時処理
            instance.OnInitializeAsync()
            .SubscribeWithState(instance, (_, i) =>
            {
                // トグルが一つもオンでない状態を許可するか。
                i.toggleGroup.allowSwitchOff = allowSwitchOff;
            });

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
        /// <summary> トグルグループ追加 </summary>
        public static IIgniteGUIGroup AddToggleGroup(this IIgniteGUIGroup group, bool allowSwitchOff = false, Action<IgniteToggleGroup> onInitialize = null, string id = "")
        {
            return group.Add(IgniteToggleGroup.Create(allowSwitchOff, onInitialize, id));
        }
    }
}