using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteButton : IgniteGUIElement
    {
        [SerializeField] Text text;
        [SerializeField] Button button;
        [SerializeField] Image image;

        public IObservable<Unit> OnClick { get { return button.OnClickAsObservable(); } }

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
                RectTransform.SetSizeDelta(image.rectTransform.rect.width, height.Value);
                image.rectTransform.SetSizeDelta(y: height.Value);
            }

            if (fontSize.HasValue)
            {
                text.fontSize = (int)fontSize.Value;
            }

            RectTransform.SetSizeDelta(x: image.rectTransform.sizeDelta.x);
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.Button, theme.ButtonTransition, theme.Font);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? buttonColor = null, ColorBlock? buttonTransitonColor = null, Color? fontColor = null)
        {
            if (buttonColor.HasValue) image.color = buttonColor.Value;
            if (buttonTransitonColor.HasValue) button.colors = buttonTransitonColor.Value;
            if (fontColor.HasValue) text.color = fontColor.Value;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return button.OnClickAsObservable();
        }

        /// <summary> ボタン生成 </summary>
        public static IgniteButton Create(string name, Action<Unit> onClick = null, Action<IgniteButton> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Button")).GetComponent<IgniteButton>();

            instance.text.text = name;  // ボタン名設定
            instance.ID        = id;    // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
            .SubscribeWithState(instance, (_, i) =>
            {
                i.image.ObserveEveryValueChanged(img => img.rectTransform.sizeDelta.x)
                       .SubscribeWithState(i, (x, button) => button.RectTransform.SetSizeDelta(x)).AddTo(i);
            });

            // クリックイベントがあれば登録
            if (onClick != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, onClick, (_, i, click) =>
                {
                    i.button.OnClickAsObservable().Subscribe(click);
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
        /// <summary> ボタン追加 </summary>
        public static IIgniteGUIGroup AddButton(this IIgniteGUIGroup group, string buttonName, Action<Unit> onClick = null, Action<IgniteButton> onInitialize = null, string id = "")
        {
            return group.Add(IgniteButton.Create(buttonName, onClick, onInitialize, id));
        }

        /// <summary> ボタン追加(ラベル付き) </summary>
        public static IIgniteGUIGroup AddButton(this IIgniteGUIGroup group,string label, string buttonName, Action<Unit> onClick = null, Action<IgniteButton> onInitialize = null, string id = "")
        {
            return group.Add(IgniteHorizontalGroup.Create().AddButton(buttonName, onClick, onInitialize, id).AddLabel(label) as IgniteHorizontalGroup);
        }
    }
}