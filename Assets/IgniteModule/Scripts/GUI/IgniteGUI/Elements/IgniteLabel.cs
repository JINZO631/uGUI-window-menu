using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteLabel : IgniteGUIElement
    {
        [SerializeField] Image background;
        [SerializeField] Text text;
        [SerializeField] Color backgroundColor;

        /// <summary> テキスト </summary>
        public Text TextMesh { get { return text; } }

        /// <summary> 表示テキスト </summary>
        public string Text
        {
            get { return text.text; }
            set { text.text = value; }
        }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            SetSize(size.FontSize);
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(float fontSize)
        {
            text.fontSize = (int)fontSize;
            RectTransform.SetSizeDelta(x: text.rectTransform.sizeDelta.x);
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.Font, theme.LabelHighlitedColor);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? fontColor = null, Color? labelHighlitedColor = null)
        {
            if (fontColor.HasValue) text.color = fontColor.Value;
            if (labelHighlitedColor.HasValue) backgroundColor = labelHighlitedColor.Value;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return background.OnPointerClickAsObservable().AsUnitObservable();
        }

        /// <summary> ラベル生成 </summary>
        public static IgniteLabel Create(string label, IObservable<string> observableLabel = null, Action<IgniteLabel> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Label")).GetComponent<IgniteLabel>();

            instance.text.text = label;  // ラベル設定
            instance.ID        = id;     // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
            .SubscribeWithState(instance, (_, i) =>
            {
                // カーソルがラベルに合わさった際背景色を変更する
                i.background.OnPointerEnterAsObservable().SubscribeWithState(i, (__, l) => l.background.color = l.backgroundColor);
                i.background.OnPointerExitAsObservable().SubscribeWithState(i, (__, l) => l.background.color = Color.clear);
            });

            // ラベル変更処理
            if (observableLabel != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, observableLabel, (_, i, l) =>
                {
                    l.SubscribeToText(i.text).AddTo(i);
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
        /// <summary> ラベル追加 </summary>
        public static IIgniteGUIGroup AddLabel(this IIgniteGUIGroup group, string label, IObservable<string> observableLabel = null, Action<IgniteLabel> onInitialize = null, string id = "")
        {
            return group.Add(IgniteLabel.Create(label, observableLabel, onInitialize, id));
        }
    }
}