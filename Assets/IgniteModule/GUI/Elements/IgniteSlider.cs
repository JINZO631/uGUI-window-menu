using UnityEngine;
using UnityEngine.UI;
using IgniteModule.GUICore;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using System.Collections;

namespace IgniteModule
{
    public class IgniteSlider : IgniteGUIElement, IPointerClickHandler
    {
        [SerializeField] Slider slider = null;
        [SerializeField] RectTransform handleAreaRect = null;
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Image handleImage = null;
        [SerializeField] Text sliderValueText = null;
        [SerializeField] HorizontalLayoutGroup horizontalLayoutGroup = null;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onSelected.Invoke();
        }

        public void SetSize(float width, float height)
        {
            horizontalLayoutGroup.padding = new RectOffset(0, (int)height, 0, 0);
            this.RectTransform.SetSizeDelta(width, height);
            handleAreaRect.SetSizeDelta(y: height);
            handleImage.rectTransform.SetSizeDelta(x: height);
        }

        public static IgniteSlider Create(Action<float> onValueChanged, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, UnityEvent<float> valueChangeEvent = null)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Slider")).GetComponent<IgniteSlider>();

            instance.SetSize(IgniteGUISettings.ElementWidth, IgniteGUISettings.ElementHeight);
            instance.backgroundImage.color = IgniteGUISettings.SliderBackgroundColor;
            instance.handleImage.color = IgniteGUISettings.SliderHandleColor;
            instance.sliderValueText.font = IgniteGUISettings.Font;
            instance.sliderValueText.fontSize = IgniteGUISettings.FontSize;
            instance.sliderValueText.color = IgniteGUISettings.FontColor;
            instance.slider.minValue = minValue;
            instance.slider.maxValue = maxValue;
            instance.slider.wholeNumbers = wholeNumbers;
            instance.slider.onValueChanged.AddListener(new UnityAction<float>(onValueChanged));
            instance.slider.onValueChanged.AddListener(v => instance.sliderValueText.text = v.ToString("F"));

            if (valueChangeEvent != null)
            {
                valueChangeEvent.AddListener(v => instance.slider.value = v);
            }

            return instance;
        }

        public class ValueChangeEvent : UnityEvent<float>
        {
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddSlider(this IIgniteGUIGroup group, Action<float> onValueChanged, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, UnityEvent<float> valueChangeEvent = null)
        {
            return group.Add(IgniteSlider.Create(onValueChanged, minValue, maxValue, wholeNumbers, valueChangeEvent));
        }

        public static IIgniteGUIGroup AddSlider(this IIgniteGUIGroup group, string label, Action<float> onValueChanged, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, UnityEvent<float> valueChangeEvent = null)
        {
            return group.Add(IgniteHorizontalGroup.Create().AddLabel(label).Add(IgniteSlider.Create(onValueChanged, minValue, maxValue, wholeNumbers, valueChangeEvent)) as IgniteHorizontalGroup);
        }

        public static IIgniteGUIGroup AddMonitoringSlider(this IIgniteGUIGroup group, Func<float> monitor, float minValue = 0f, float maxValue = 1f)
        {
            var behaviour = (MonoBehaviour)(group);
            var valueChangeEvent = new IgniteSlider.ValueChangeEvent();
            behaviour.StartCoroutine(MonitoringCoroutine(valueChangeEvent, monitor));
            return group.AddSlider(v => { }, minValue: minValue, maxValue: maxValue, valueChangeEvent: valueChangeEvent);
        }

        public static IIgniteGUIGroup AddMonitoringSlider(this IIgniteGUIGroup group, string label, Func<float> monitor, float minValue = 0f, float maxValue = 1f)
        {
            var behaviour = (MonoBehaviour)(group);
            var valueChangeEvent = new IgniteSlider.ValueChangeEvent();
            behaviour.StartCoroutine(MonitoringCoroutine(valueChangeEvent, monitor));
            return group.AddSlider(label, v => { }, minValue: minValue, maxValue: maxValue, valueChangeEvent: valueChangeEvent);
        }

        static IEnumerator MonitoringCoroutine(UnityEvent<float> valueChangeEvent, Func<float> monitor)
        {
            while (true)
            {
                valueChangeEvent.Invoke(monitor());
                yield return null;
            }
        }
    }
}