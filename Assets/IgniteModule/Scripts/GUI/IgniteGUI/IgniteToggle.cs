using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteToggle : UIMonoBehaviour, IIgniteGUIElement
    {
        [SerializeField] Toggle toggle;
        [SerializeField] Image background;
        [SerializeField] Image checkmark;
        [SerializeField] Color disableColor;
        [SerializeField] Color enableColor;

        public string ID { get; private set; }
        public IgniteWindow Window { get; private set; }
        public ReadOnlyReactiveProperty<bool> Value { get; private set; }

        protected override void Start()
        {
            toggle.OnValueChangedAsObservable()
                  .Subscribe(v => checkmark.enabled = v);
        }

        public void SetSize(IIgniteGUISize size)
        {
            RectTransform.sizeDelta = size.ToggleSize;
            background.rectTransform.sizeDelta = size.ToggleSize;
            checkmark.rectTransform.sizeDelta = size.ToggleSize;
            checkmark.rectTransform.SetLocalScale(0.8f, 0.8f);
        }

        public void SetTheme(IIgniteGUITheme theme)
        {
            background.color = theme.ToggleDisable;
            checkmark.color = theme.ToggleEnable;
        }

        public IObservable<Unit> OnSelected()
        {
            return toggle.OnValueChangedAsObservable().AsUnitObservable();
        }

        public static IgniteToggle Create(IgniteWindow window, Action<bool> onValueChanged = null, Action<IObservable<bool>> doSubscribe = null, string id = "")
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Toggle")).GetComponent<IgniteToggle>();
            instance.Value = instance.toggle.OnValueChangedAsObservable().ToReadOnlyReactiveProperty();
            instance.Window = window;
            if (onValueChanged != null)
            {
                instance.toggle.OnValueChangedAsObservable().Subscribe(onValueChanged);
            }
            if (doSubscribe != null)
            {
                doSubscribe(instance.Value);
            }
            instance.ID = id;
            return instance;
        }
    }

    public static class IgniteToggleExtensions
    {
        public static IIgniteGUIGroup AddToggle(this IIgniteGUIGroup group, Action<bool> onValueChanged = null, Action<IObservable<bool>> doSubscribe = null, string id = "")
        {
            return group.Add(IgniteToggle.Create(group.Window, onValueChanged, doSubscribe, id));
        }

        public static IIgniteGUIGroup AddToggle(this IIgniteGUIGroup group, string label, Action<bool> onValueChanged = null, Action<IObservable<bool>> doSubscribe = null, string id = "")
        {
            return group.Add(IgniteHorizontalGroup.Create(group).AddToggle(onValueChanged, doSubscribe, id).AddLabel(label) as IgniteHorizontalGroup);
        }
    }
}