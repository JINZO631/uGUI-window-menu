using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteSlider : UIMonoBehaviour, IIgniteGUIElement
    {
        [SerializeField] Slider slider;
        [SerializeField] Image background;
        [SerializeField] RectTransform handleSliderArea;
        [SerializeField] Image handle;
        [SerializeField] TextMeshProUGUI text;

        public string ID { get; private set; }
        public IgniteWindow Window { get; private set; }
        public Slider Slider { get { return slider; } }
        public ReadOnlyReactiveProperty<float> Value { get; private set; }

        protected override void Start()
        {
            slider.OnValueChangedAsObservable().Subscribe(v => text.text = v.ToString("F"));
        }

        public void SetSize(IIgniteGUISize size)
        {
            RectTransform.SetSizeDelta(x: size.SliderWidth, y: size.ElementHeight);
            background.rectTransform.sizeDelta = Vector2.zero;
            handle.rectTransform.SetSizeDelta(x: size.ElementHeight);
            handle.rectTransform.SetAnchoredPosition(x: size.ElementHeight * 0.5f);
            handleSliderArea.SetSizeDelta(x: -size.ElementHeight);
            handleSliderArea.SetAnchoredPosition(x: size.ElementHeight * -0.5f);
        }

        public void SetTheme(IIgniteGUITheme theme)
        {
            background.color = theme.SliderBackground;
            handle.color = theme.SliderHandle;
            text.color = theme.SliderText;
        }

        public IObservable<Unit> OnSelected()
        {
            return slider.OnPointerClickAsObservable().AsUnitObservable();
        }

        static IgniteSlider Create()
        {
            var instance =  Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Slider")).GetComponent<IgniteSlider>();
            instance.Value = instance.slider.OnValueChangedAsObservable().ToReadOnlyReactiveProperty();
            return instance;
        }

        public static IgniteSlider Create(IgniteWindow window, Action<float> onValueChanged = null, Action<IObservable<float>> doSubscribe = null, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, string id = "")
        {
            var instance = Create();
            instance.Window = window;
            if (onValueChanged != null)
            {
                instance.slider.OnValueChangedAsObservable().Subscribe(onValueChanged);
            }
            if (doSubscribe != null)
            {
                doSubscribe(instance.Value);
            }
            instance.slider.minValue = minValue;
            instance.slider.maxValue = maxValue;
            instance.slider.wholeNumbers = wholeNumbers;
            instance.ID = id;
            return instance;
        }
    }

    public static class IgntieSliderExtensions
    {
        public static IIgniteGUIGroup AddSlider(this IIgniteGUIGroup group, Action<float> onValueChanged = null, Action<IObservable<float>> doSubscribe = null, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, string id = "")
        {
            return group.Add(IgniteSlider.Create(group.Window, onValueChanged, doSubscribe, minValue, maxValue, wholeNumbers, id));
        }

        public static IIgniteGUIGroup AddSlider(this IIgniteGUIGroup group, string label, Action<float> onValueChanged = null, Action<IObservable<float>> doSubscribe = null, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, string id = "")
        {
            return group.Add(IgniteHorizontalGroup.Create(group).AddLabel(label).AddSlider(onValueChanged, doSubscribe, minValue, maxValue, wholeNumbers, id) as IgniteHorizontalGroup);
        }
    }
}