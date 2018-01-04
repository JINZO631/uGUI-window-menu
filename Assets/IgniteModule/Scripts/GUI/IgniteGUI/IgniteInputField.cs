using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace IgniteModule.UI
{
    public class IgniteInputField : UIMonoBehaviour, IIgniteGUIElement
    {
        [SerializeField] TMP_InputField inputField;

        public string ID { get; private set; }
        public IgniteWindow Window { get; private set; }
        public ReadOnlyReactiveProperty<string> Value { get; private set; }

        public void SetSize(IIgniteGUISize size)
        {
            RectTransform.SetSizeDelta(size.InputFieldWidth, size.ElementHeight);

        }

        public void SetTheme(IIgniteGUITheme theme)
        {
        }

        public IObservable<Unit> OnSelected()
        {
            return inputField.OnPointerClickAsObservable().AsUnitObservable();
        }

        public static IgniteInputField Create(IgniteWindow window, Action<string> onValueChanged = null, Action<string> onEndEdit = null, TMP_InputField.CharacterValidation characterValidation = TMP_InputField.CharacterValidation.None, bool readOnly = false,  string id = "")
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/InputField")).GetComponent<IgniteInputField>();
            instance.Window = window;
            instance.Value = instance.inputField.OnValueChangedAsObservable().ToReadOnlyReactiveProperty();
            if (onValueChanged != null)
            {
                instance.inputField.OnValueChangedAsObservable().Subscribe(onValueChanged);
            }
            if (onEndEdit != null)
            {
                instance.inputField.OnEndEditAsObservable().Subscribe(onEndEdit);
            }
            instance.inputField.characterValidation = characterValidation;
            instance.inputField.readOnly = readOnly;
            instance.ID = id;
            return instance;
        }
    }

    public static class IgniteInputFieldExtensions
    {
        public static IIgniteGUIGroup AddInputField(this IIgniteGUIGroup group, Action<string> onValueChanged = null, Action<string> onEndEdit = null, TMP_InputField.CharacterValidation characterValidation = TMP_InputField.CharacterValidation.None, bool readOnly = false, string id = "")
        {
            return group.Add(IgniteInputField.Create(group.Window, onValueChanged, onEndEdit, characterValidation, readOnly, id));
        }

        public static IIgniteGUIGroup AddInputField(this IIgniteGUIGroup group, string label, Action<string> onValueChanged = null, Action<string> onEndEdit = null, TMP_InputField.CharacterValidation characterValidation = TMP_InputField.CharacterValidation.None, bool readOnly = false, string id = "")
        {
            return group.Add(IgniteHorizontalGroup.Create(group).AddLabel(label).AddInputField(onValueChanged, onEndEdit, characterValidation, readOnly, id) as IgniteHorizontalGroup);
        }
    }
}