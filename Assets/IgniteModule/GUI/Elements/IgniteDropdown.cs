using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Linq;
using IgniteModule.GUICore;

namespace IgniteModule
{
    public class IgniteDropdown : IgniteGUIElement, IPointerClickHandler
    {
        [SerializeField] Dropdown dropdown = null;
        [SerializeField] RectTransform arrowRect = null;
        [SerializeField] RectTransform templateItem = null;
        [SerializeField] Text templateLabel = null;
        [SerializeField] RectTransform templateToggleRect = null;
        [SerializeField] RectTransform templateCheckmarkRect = null;
        [SerializeField] RectTransform templateContentRect = null;
        [SerializeField] HorizontalLayoutGroup templateItemLayoutGroup = null;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onSelected.Invoke();
        }

        public void SetHeight(float height)
        {
            RectTransform.SetSizeDelta(y: height);
            arrowRect.SetSizeDelta(height, height);
            arrowRect.SetAnchoredPosition(x: -height);
            templateItem.SetSizeDelta(y: height);
            templateToggleRect.SetSizeDelta(y: height);
            templateCheckmarkRect.SetSizeDelta(x: height);
            templateContentRect.SetSizeDelta(y: height * dropdown.options.Count);
            templateItemLayoutGroup.padding = new RectOffset((int)height, 0, 0, 0);
            dropdown.template.SetSizeDelta(y: height * dropdown.options.Count);
        }

        public static IgniteDropdown Create(Action<int> onValueChanged, params string[] options)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Dropdown")).GetComponent<IgniteDropdown>();

            instance.dropdown.captionText.font = IgniteGUISettings.Font;
            instance.dropdown.captionText.fontSize = IgniteGUISettings.FontSize;
            instance.templateLabel.font = IgniteGUISettings.Font;
            instance.templateLabel.fontSize = IgniteGUISettings.FontSize;
            instance.dropdown.AddOptions(options.ToList());
            instance.dropdown.onValueChanged.AddListener(new UnityAction<int>(onValueChanged));
            instance.SetHeight(IgniteGUISettings.ElementHeight);

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddDropdown(this IIgniteGUIGroup group, Action<int> onValueChanged, params string[] options)
        {
            return group.Add(IgniteDropdown.Create(onValueChanged, options));
        }
    }
}