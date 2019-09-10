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
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Text labelText = null;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onSelected.Invoke();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            backgroundImage.color = IgniteGUISettings.LabelHighlightColor;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            backgroundImage.color = Color.clear;
        }

        public static IgniteLabel Create(string label, UnityEvent<string> labelChangeEvent = null, Color? fontColor = null)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Label")).GetComponent<IgniteLabel>();

            instance.labelText.text = label;
            instance.labelText.font = IgniteGUISettings.Font;
            instance.labelText.fontSize = IgniteGUISettings.FontSize;
            instance.labelText.resizeTextMaxSize = IgniteGUISettings.FontSize;
            instance.labelText.color = fontColor ?? IgniteGUISettings.FontColor;

            if (labelChangeEvent != null)
            {
                labelChangeEvent.AddListener(latestLabel => instance.labelText.text = latestLabel);
            }

            return instance;
        }

        public class LabelChangeEvent : UnityEvent<string>
        {
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddLabel(this IIgniteGUIGroup group, string label, UnityEvent<string> labelChangeEvent = null, Color? fontColor = null)
        {
            return group.Add(IgniteLabel.Create(label, labelChangeEvent, fontColor));
        }

        public static IIgniteGUIGroup AddMonitoringLabel(this IIgniteGUIGroup group, Func<string> monitor)
        {
            var labelChangeEvent = new IgniteLabel.LabelChangeEvent();
            var label = IgniteLabel.Create("", labelChangeEvent);
            label.StartCoroutine(MonitoringCoroutine(labelChangeEvent, monitor));
            return group.AddLabel("", labelChangeEvent);
        }

        static IEnumerator MonitoringCoroutine(UnityEvent<string> labelChangeEvent, Func<string> monitor)
        {
            while (true)
            {
                labelChangeEvent.Invoke(monitor());
                yield return null;
            }
        }
    }
}