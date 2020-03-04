using System;
using IgniteModule.GUICore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IgniteModule
{
    public class IgniteToggle : IgniteGUIElement
    {
        [SerializeField] Toggle toggle = null;
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Image checkmarkImage = null;
        [SerializeField] LayoutElement layoutElement = null;

        public bool IsOn => toggle.isOn;

        public void SetHeight(float height)
        {
            layoutElement.minHeight = height;
            layoutElement.minWidth = height;
            RectTransform.SetSizeDelta(height, height);
            backgroundImage.rectTransform.SetSizeDelta(height, height);
        }

        public static IgniteToggle Create(Action<bool> onValueChanged, bool defaultValue, bool readOnly)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Toggle")).GetComponent<IgniteToggle>();

            instance.SetHeight(IgniteGUISettings.ElementHeight);
            instance.backgroundImage.color = IgniteGUISettings.ToggleBackgroundColor;
            instance.checkmarkImage.color = IgniteGUISettings.ToggleEnableColor;
            instance.toggle.isOn = defaultValue;
            instance.toggle.onValueChanged.AddListener(new UnityAction<bool>(onValueChanged));
            instance.toggle.onValueChanged.AddListener(v => instance.onSelected.Invoke());
            instance.toggle.onValueChanged.AddListener(v => instance.checkmarkImage.enabled = v);
            instance.toggle.interactable = !readOnly;

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddToggle(this IIgniteGUIGroup group, Action<bool> onValueChanged, bool defaultValue = true, bool readOnly = false)
        {
            return group.Add(IgniteToggle.Create(onValueChanged, defaultValue, readOnly));
        }

        public static IIgniteGUIGroup AddToggle(this IIgniteGUIGroup group, string label, Action<bool> onValueChanged, bool defaultValue = true, bool readOnly = false)
        {
            return group.Add(IgniteHorizontalGroup.Create().AddToggle(onValueChanged, defaultValue, readOnly).AddLabel(label) as IgniteHorizontalGroup);
        }

        public static IIgniteGUIGroup AddToggleWithButton(this IIgniteGUIGroup group, string buttonName, Action<bool> onClick, Action<bool> onValueChanged = null, bool defaultValue = true, bool readOnly = false)
        {
            var toggle = IgniteToggle.Create(onValueChanged ?? delegate { }, defaultValue, readOnly);

            var horizontalGroup = IgniteHorizontalGroup.Create().Add(toggle).AddButton(buttonName, () => onClick(toggle.IsOn)) as IgniteHorizontalGroup;

            return group.Add(horizontalGroup);
        }
    }
}