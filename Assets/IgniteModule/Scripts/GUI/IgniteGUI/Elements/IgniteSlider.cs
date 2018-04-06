using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteSlider : IgniteGUIElement
    {
        [SerializeField] Slider slider;
        [SerializeField] Image background;
        [SerializeField] RectTransform handleSliderArea;
        [SerializeField] Image handle;
        [SerializeField] Text text;

        /// <summary> スライダー </summary>
        public Slider Slider { get { return slider; } }
        /// <summary> スライダーの値 </summary>
        public ReadOnlyReactiveProperty<float> Value { get; private set; }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            SetSize(size.SliderWidth, size.ElementHeight, size.FontSize);
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(float? width = null, float? height = null, float? fontSize = null)
        {
            background.rectTransform.sizeDelta = Vector2.zero;
            if (width.HasValue)
            {
                RectTransform.SetSizeDelta(x: width.Value);
            }

            if (height.HasValue)
            {
                RectTransform.SetSizeDelta(y: height.Value);

                handle.rectTransform.SetSizeDelta(x: height.Value);
                handle.rectTransform.SetAnchoredPosition(x: height.Value * 0.5f);
                handleSliderArea.SetSizeDelta(x: -height.Value);
                handleSliderArea.SetAnchoredPosition(x: height.Value * -0.5f);
            }

            if (fontSize.HasValue)
            {
                text.fontSize = (int)fontSize.Value;
            }
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.SliderBackground, theme.SliderHandle, theme.SliderText);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? backgroundColor = null, Color? handleColor = null, Color? fontColor = null)
        {
            if (backgroundColor.HasValue) background.color = backgroundColor.Value;
            if (handleColor.HasValue) handle.color = handleColor.Value;
            if (fontColor.HasValue) text.color = fontColor.Value;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return slider.OnPointerClickAsObservable().AsUnitObservable();
        }

        /// <summary> 生成 </summary>
        public static IgniteSlider Create(Action<float> onValueChanged = null, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, IObservable<float> observableValue = null, Action<IgniteSlider> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Slider")).GetComponent<IgniteSlider>();

            instance.slider.minValue = minValue;      // 最小値設定
            instance.slider.maxValue = maxValue;      // 最大値設定
            instance.slider.wholeNumbers = wholeNumbers;  // 整数のみかどうか設定
            instance.ID = id;            // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
            .SubscribeWithState(instance, (_, i) =>
            {
                // スライダーの値を元にReactivePropertyを生成
                i.Value = i.slider.OnValueChangedAsObservable().ToReadOnlyReactiveProperty();
                // スライダーの値が変更されたらTextの表示を変更
                i.slider.OnValueChangedAsObservable().Select(v => v.ToString("F")).SubscribeToText(i.text);
            });

            // 値変更時イベント
            if (onValueChanged != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, onValueChanged, (_, i, onVC) =>
                {
                    i.slider.OnValueChangedAsObservable().Subscribe(onVC);
                });
            }

            // observableな値
            if (observableValue != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, observableValue, (_, i, value) =>
                {
                    value.SubscribeWithState(instance, (v, s) => s.slider.value = v).AddTo(i);
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
        /// <summary> スライダー追加 </summary>
        public static IIgniteGUIGroup AddSlider(this IIgniteGUIGroup group, string label = null, Action<float> onValueChanged = null, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, IObservable<float> observableValue = null, Action<IgniteSlider> onInitialize = null, string id = "")
        {
            if (label != null)
            {
                return group.Add(IgniteHorizontalGroup.Create().AddLabel(label).Add(IgniteSlider.Create(onValueChanged, minValue, maxValue, wholeNumbers, observableValue, onInitialize, id)) as IgniteHorizontalGroup);
            }
            else
            {
                return group.Add(IgniteSlider.Create(onValueChanged, minValue, maxValue, wholeNumbers, observableValue, onInitialize, id));
            }
        }
    }
}