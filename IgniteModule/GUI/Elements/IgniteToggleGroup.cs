using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IgniteModule
{
    public class IgniteToggleGroup : IgniteGUIElementGroup
    {
        [SerializeField] ToggleGroup toggleGroup = null;

        public override RectTransform Content => this.Parent.Content;

        public override IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            base.Add(element);
            StartCoroutine(DelayAdd(element));
            return this;
        }

        IEnumerator DelayAdd(IIgniteGUIElement element)
        {
            yield return null;
            RegisterToggle(element);
        }

        void RegisterToggle(IIgniteGUIElement element)
        {
            var toggle = element as IgniteToggle;
            if (toggle != null)
            {
                RegisterToggle(toggle);
                return;
            }

            var group = element as IIgniteGUIGroup;
            if (group != null)
            {
                var toggles = group.Content.GetComponentsInChildren<IgniteToggle>();
                if (toggles != null)
                {
                    foreach (var i in toggles)
                    {
                        RegisterToggle(i);
                    }
                }
            }
        }

        void RegisterToggle(IgniteToggle toggle)
        {
            var t = toggle.GetComponentInChildren<Toggle>();
            t.group = toggleGroup;
        }

        public static IgniteToggleGroup Create(bool allowSwitchOff = false)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/ToggleGroup")).GetComponent<IgniteToggleGroup>();

            instance.toggleGroup.allowSwitchOff = allowSwitchOff;
            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddToggleGroup(this IIgniteGUIGroup group, bool allowSwitchOff = false)
        {
            return group.Add(IgniteToggleGroup.Create(allowSwitchOff));
        }
    }
}