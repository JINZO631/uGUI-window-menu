using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteToggle : IgniteGUIElement
    {
        [SerializeField] Toggle toggle;
        [SerializeField] Image background;
        [SerializeField] Image checkmark;

        /// <summary> トグル </summary>
        public Toggle Toggle { get { return toggle; } }
        /// <summary> トグルの値 </summary>
        public ReadOnlyReactiveProperty<bool> Value { get; private set; }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            SetSize(size.ToggleSize);
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(Vector2 toggleSize)
        {
            RectTransform.sizeDelta = toggleSize;
            background.rectTransform.sizeDelta = toggleSize;
            checkmark.rectTransform.sizeDelta = toggleSize;
            checkmark.rectTransform.SetLocalScale(0.8f, 0.8f);
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.ToggleEnable, theme.ToggleDisable);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? enable = null, Color? disable = null)
        {
            if (enable.HasValue) checkmark.color = enable.Value;
            if (disable.HasValue) background.color = disable.Value;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return toggle.OnValueChangedAsObservable().Skip(1).AsUnitObservable();
        }

        /// <summary> 生成 </summary>
        public static IgniteToggle Create(Action<bool> onValueChanged = null, Action<IgniteToggle> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Toggle")).GetComponent<IgniteToggle>();

            instance.ID = id;  // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
            .SubscribeWithState(instance, (_, i) =>
            {
                //  トグルの値を元にReactivePropertyを生成
                i.Value = i.toggle.OnValueChangedAsObservable().ToReadOnlyReactiveProperty();

                // トグルの値が変わったらチェックマークの表示を切り替える
                i.toggle.OnValueChangedAsObservable()
                        .SubscribeWithState(i, (v, toggle) => toggle.checkmark.enabled = v);
            });

            // 値変更時イベント
            if (onValueChanged != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, onValueChanged, (_, i, onVC) =>
                {
                    i.toggle.OnValueChangedAsObservable().Skip(1).Subscribe(onVC);
                });
            }

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
        /// <summary> トグル追加 </summary>
        public static IIgniteGUIGroup AddToggle(this IIgniteGUIGroup group, Action<bool> onValueChanged = null, Action<IgniteToggle> onInitialize = null, string id = "")
        {
            return group.Add(IgniteToggle.Create(onValueChanged, onInitialize, id));
        }

        /// <summary> トグル追加 </summary>
        public static IIgniteGUIGroup AddToggle(this IIgniteGUIGroup group, string label, Action<bool> onValueChanged = null, Action<IgniteToggle> onInitialize = null, string id = "")
        {
            return group.Add(IgniteHorizontalGroup.Create().AddToggle(onValueChanged, onInitialize, id).AddLabel(label) as IgniteHorizontalGroup);
        }

        /// <summary> トグルとボタン追加 </summary>
        public static IIgniteGUIGroup AddToggleWithButton(this IIgniteGUIGroup group, string buttonName, Action<bool> onClick, Action<bool> onValueChanged = null, Action<IgniteToggle> onInitialize = null, string id = "")
        {
            var toggle = IgniteToggle.Create(onValueChanged, onInitialize, id);

            Action<Unit> buttonClick = buttonClick = _ => onClick(toggle.Value.Value);

            var horizontalGroup = IgniteHorizontalGroup.Create().Add(toggle).AddButton(buttonName, buttonClick) as IgniteHorizontalGroup;

            return group.Add(horizontalGroup);
        }
    }
}