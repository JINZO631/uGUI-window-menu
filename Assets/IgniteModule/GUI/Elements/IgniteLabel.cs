using UnityEngine;
using IgniteModule.GUICore;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

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
            backgroundImage.color = new Color(1, 1, 1, 0.5f);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            backgroundImage.color = Color.clear;
        }

        public static IgniteLabel Create(string label, UnityEvent<string> labelChangeEvent = null)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Label")).GetComponent<IgniteLabel>();

            instance.labelText.text = label;
            instance.labelText.font = IgniteGUISettings.Font;
            instance.labelText.fontSize = IgniteGUISettings.FontSize;

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
        public static IIgniteGUIGroup AddLabel(this IIgniteGUIGroup group, string label, UnityEvent<string> labelChangeEvent = null)
        {
            return group.Add(IgniteLabel.Create(label, labelChangeEvent));
        }
    }
}