using UnityEngine;
using IgniteModule.GUICore;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Collections;

namespace IgniteModule
{
    public class IgniteButton : IgniteGUIElement
    {
        [SerializeField] Button button = null;
        [SerializeField] Text buttonNameText = null;
        [SerializeField] Image backgroundImage = null;

        IEnumerator Fit()
        {
            yield return null;
            RectTransform.SetSizeDelta(x: backgroundImage.rectTransform.sizeDelta.x);
        }

        public static IgniteButton Create(string buttonName, Action onClick)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Button")).GetComponent<IgniteButton>();

            instance.buttonNameText.text = buttonName;
            instance.buttonNameText.font = IgniteGUISettings.Font;
            instance.buttonNameText.fontSize = IgniteGUISettings.FontSize;
            instance.buttonNameText.resizeTextMaxSize = IgniteGUISettings.FontSize;
            instance.buttonNameText.color = IgniteGUISettings.FontColor;
            instance.backgroundImage.rectTransform.SetSizeDelta(y: IgniteGUISettings.ElementHeight);
            instance.backgroundImage.color = IgniteGUISettings.ButtonColor;
            instance.RectTransform.SetSizeDelta(y: IgniteGUISettings.ElementHeight);
            instance.button.onClick.AddListener(new UnityAction(onClick));
            instance.StartCoroutine(instance.Fit());

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddButton(this IIgniteGUIGroup group, string buttonName, Action onClick)
        {
            return group.Add(IgniteButton.Create(buttonName, onClick));
        }

        public static IIgniteGUIGroup AddButton(this IIgniteGUIGroup group, string label, string buttonName, Action onClick)
        {
            return group.Add(IgniteHorizontalGroup.Create().AddButton(buttonName, onClick).AddLabel(label) as IgniteHorizontalGroup);
        }
    }
}