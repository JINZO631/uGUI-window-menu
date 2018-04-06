using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteDropdown : IgniteGUIElement
    {
        [SerializeField] Dropdown dropdown;
        [SerializeField] Image image;
        [SerializeField] Text label;
        [SerializeField] Image arrow;
        [SerializeField] RectTransform templateItem;
        [SerializeField] Text templateLabel;
        [SerializeField] Image templateBackground;
        [SerializeField] Toggle templateToggle;

        /// <summary> ドロップダウン </summary>
        public Dropdown Dropdown { get { return dropdown; } }
        /// <summary> 選択中の項目(index) </summary>
        public ReadOnlyReactiveProperty<int> Value { get; private set; }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            SetSize(size.ElementHeight, size.FontSize);
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(float? height = null, float? fontSize = null)
        {
            if (height.HasValue)
            {
                RectTransform.SetSizeDelta(y: height.Value);
                templateItem.SetSizeDelta(y: height.Value);
            }
            if (fontSize.HasValue)
            {
                label.fontSize = (int)fontSize.Value;
                templateLabel.fontSize = (int)fontSize.Value;
            }
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.DropdownFont, theme.DropdownBackground, theme.DropdownOptionsFont, theme.DropdownOptionsBackground, theme.DropdownCheckmark);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? fontColor = null, Color ? backgroundColor = null, Color? optionsFontColor = null, Color? optionsBackgroundColor = null, Color? checkmarkColor = null)
        {
            if (fontColor.HasValue)
            {
                label.color = fontColor.Value;
                arrow.color = fontColor.Value;
            }
            if (backgroundColor.HasValue) image.color = backgroundColor.Value;
            if (optionsFontColor.HasValue) templateLabel.color = optionsFontColor.Value;
            if (optionsBackgroundColor.HasValue) templateBackground.color = optionsBackgroundColor.Value;
            if (checkmarkColor.HasValue) templateToggle.graphic.color = checkmarkColor.Value;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return dropdown.OnPointerClickAsObservable().AsUnitObservable();
        }

        /// <summary> 生成 </summary>
        public static IgniteDropdown Create(string[] options, Action<int> onValueChanged = null, Action<IgniteDropdown> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Dropdown")).GetComponent<IgniteDropdown>();

            instance.ID = id;  // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
            .SubscribeWithState2(instance, options, (_, i, opt) =>
            {
                // ドロップダウンの値を元にReactivePropertyを生成
                i.Value = i.dropdown.OnValueChangedAsObservable().ToReadOnlyReactiveProperty();
                // ドロップダウンの項目を設定
                i.dropdown.AddOptions(opt.ToList());
            });

            // 値変更イベント
            if (onValueChanged != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, onValueChanged, (_, i, onVC) =>
                {
                    i.Value.Subscribe(onVC);
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
        /// <summary> ドロップダウン追加 </summary>
        public static IIgniteGUIGroup AddDropdown(this IIgniteGUIGroup group, string[] options, Action<int> onValueChanged = null, Action<IgniteDropdown> onInitialize = null, string id = "")
        {
            return group.Add(IgniteDropdown.Create(options, onValueChanged, onInitialize, id));
        }
    }
}