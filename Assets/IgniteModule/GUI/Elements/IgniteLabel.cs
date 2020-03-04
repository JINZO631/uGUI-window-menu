using UnityEngine;
using IgniteModule.GUICore;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using System.Collections;

namespace IgniteModule
{
    public class IgniteLabel : IgniteGUIElement, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public Image backgroundImage = null;
        [SerializeField] public Text labelText = null;

        public Color defaultBackgroundColor = default(Color);
        public Color highlightBackgroundColor = default(Color);

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onSelected.Invoke();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            backgroundImage.color = highlightBackgroundColor;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            backgroundImage.color = defaultBackgroundColor;
        }

        public static IgniteLabel Create(string label, Color? fontColor = null, Color? defaultBackgroundColor = null, Color? highlightBackgroundColor = null)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Label")).GetComponent<IgniteLabel>();

            instance.labelText.text = label;
            instance.labelText.font = IgniteGUISettings.Font;
            instance.labelText.fontSize = IgniteGUISettings.FontSize;
            instance.labelText.resizeTextMaxSize = IgniteGUISettings.FontSize;
            instance.labelText.color = fontColor ?? IgniteGUISettings.FontColor;
            instance.defaultBackgroundColor = defaultBackgroundColor ?? Color.clear;
            instance.highlightBackgroundColor = highlightBackgroundColor ?? IgniteGUISettings.LabelHighlightColor;

            instance.backgroundImage.color = instance.defaultBackgroundColor;

            return instance;
        }

        public class LabelChangeEvent : UnityEvent<string>
        {
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddLabel(this IIgniteGUIGroup group, string label, Color? fontColor = null, Color? defaultBackgroundColor = null, Color? highlightBackgroundColor = null)
        {
            return group.Add(IgniteLabel.Create(label, fontColor, defaultBackgroundColor, highlightBackgroundColor));
        }

        public static IIgniteGUIGroup AddHighlightedLabel(this IIgniteGUIGroup group, string label, Color highlightColor)
        {
            return group.AddLabel(label, LuminanceUtility.ChooseFontColor(highlightColor), highlightColor, highlightColor);
        }

        public static IIgniteGUIGroup AddMonitoringLabel(this IIgniteGUIGroup group, Func<string> monitor)
        {
            var labelChangeEvent = new IgniteLabel.LabelChangeEvent();
            var label = IgniteLabel.Create("");
            label.StartCoroutine(MonitoringCoroutine(() =>
            {
                if (label == null)
                {
                    return;
                }

                label.labelText.text = monitor();
            }));
            return group.Add(label);
        }

        public static IIgniteGUIGroup AddMonitoringHighligtedLabel(this IIgniteGUIGroup group, Func<string> monitor, Func<Color> colorMonitor)
        {
            var labelChangeEvent = new IgniteLabel.LabelChangeEvent();
            var label = IgniteLabel.Create("");
            label.StartCoroutine(MonitoringCoroutine(() =>
            {
                if (label == null)
                {
                    return;
                }

                var color = colorMonitor();
                label.labelText.text = monitor();
                label.labelText.color = LuminanceUtility.ChooseFontColor(color);
                label.backgroundImage.color = color;
                label.highlightBackgroundColor = color;
                label.defaultBackgroundColor = color;
            }));
            return group.Add(label);
        }

        private static IEnumerator MonitoringCoroutine(Action monitor)
        {
            while (true)
            {
                monitor();
                yield return null;
            }
        }
    }
}