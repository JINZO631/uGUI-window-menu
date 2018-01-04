using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace IgniteModule.UI
{
    public class IgniteDropdown : UIMonoBehaviour, IIgniteGUIElement
    {
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] Transform template;

        public string ID { get; private set; }
        public IgniteWindow Window { get; private set; }
        public ReadOnlyReactiveProperty<int> Value { get; private set; }

        public void SetSize(IIgniteGUISize size)
        {

        }

        public void SetTheme(IIgniteGUITheme theme)
        {

        }

        public IObservable<Unit> OnSelected()
        {
            return dropdown.OnPointerClickAsObservable().AsUnitObservable();
        }

        public static IgniteDropdown Create(IgniteWindow window, Action<int> onValueChanged = null, string id = "")
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Dropdown")).GetComponent<IgniteDropdown>();
            instance.Window = window;
            instance.Value = instance.dropdown.OnValueChangedAsObservable().ToReadOnlyReactiveProperty();
            instance.ID = id;
            return instance;
        }

        public static IgniteDropdown Create(IgniteWindow window, string[] options, Action<int> onValueChanged = null, string id = "")
        {
            var instance = Create(window, onValueChanged, id);
            instance.dropdown.AddOptions(options.ToList());
            if (onValueChanged != null)
            {
                instance.Value.Subscribe(onValueChanged);
            }
            return instance;
        }
    }

    public static class IgniteDropdownExtensions
    {
        public static IIgniteGUIGroup AddDropdown(this IIgniteGUIGroup group, string[] options, Action<int> onValueChanged = null, string id = "")
        {
            return group.Add(IgniteDropdown.Create(group.Window, options, onValueChanged, id));
        }
    }
}