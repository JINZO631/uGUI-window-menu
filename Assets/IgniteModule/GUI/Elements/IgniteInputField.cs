using UnityEngine;
using UnityEngine.UI;
using IgniteModule.GUICore;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace IgniteModule
{
    public class IgniteInputField : IgniteGUIElement, IPointerClickHandler
    {
        [SerializeField] InputField inputField = null;
        [SerializeField] Text placeHolderText = null;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onSelected.Invoke();
        }

        public void SetSize(float width, float height)
        {
            RectTransform.SetSizeDelta(width, height);
        }

        public static IgniteInputField Create(Action<string> onValueChanged = null, Action<string> onEndEdit = null, string initialValue = null, string placeHolder = null, InputField.CharacterValidation characterValidation = InputField.CharacterValidation.None, bool readOnly = false)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/InputField")).GetComponent<IgniteInputField>();

            instance.inputField.characterValidation = characterValidation;
            instance.inputField.readOnly = readOnly;
            instance.SetSize(IgniteGUISettings.ElementWidth, IgniteGUISettings.ElementHeight);
            instance.inputField.textComponent.font = IgniteGUISettings.Font;
            instance.inputField.textComponent.fontSize = IgniteGUISettings.FontSize;
            instance.placeHolderText.font = IgniteGUISettings.Font;
            instance.placeHolderText.fontSize = IgniteGUISettings.FontSize;

            if (onValueChanged != null)
            {
                instance.inputField.onValueChanged.AddListener(new UnityAction<string>(onValueChanged));
            }
            if (onEndEdit != null)
            {
                instance.inputField.onEndEdit.AddListener(new UnityAction<string>(onEndEdit));
            }
            if (initialValue != null)
            {
                instance.inputField.text = initialValue;
            }
            if (placeHolder != null)
            {
                instance.placeHolderText.text = placeHolder;
            }

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddInputFiled(this IIgniteGUIGroup group, Action<string> onValueChanged = null, Action<string> onEndEdit = null, string initialValue = null, string placeHolder = null, InputField.CharacterValidation characterValidation = InputField.CharacterValidation.None, bool readOnly = false)
        {
            return group.Add(IgniteInputField.Create(onValueChanged, onEndEdit, initialValue, placeHolder, characterValidation, readOnly));
        }

        public static IIgniteGUIGroup AddInputFiled(this IIgniteGUIGroup group, string label, Action<string> onValueChanged = null, Action<string> onEndEdit = null, string initialValue = null, string placeHolder = null, InputField.CharacterValidation characterValidation = InputField.CharacterValidation.None, bool readOnly = false)
        {
            return group.Add(IgniteHorizontalGroup.Create().AddLabel(label).AddInputFiled(onValueChanged, onEndEdit, initialValue, placeHolder, characterValidation, readOnly) as IgniteHorizontalGroup);
        }
    }
}