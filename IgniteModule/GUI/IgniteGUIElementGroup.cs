using IgniteModule.GUICore;
using UnityEngine;

namespace IgniteModule
{
    public abstract class IgniteGUIElementGroup : IgniteGUIElement, IIgniteGUIElementGroup
    {
        public IIgniteGUIGroup Parent { get; protected set; }

        public IIgniteGUIGroup LastNestedGroup { get; protected set; }

        public virtual RectTransform Content => this.RectTransform;

        public override void SetParent(IIgniteGUIGroup parent)
        {
            base.SetParent(parent);
            Parent = parent;
        }

        public virtual IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            element.SetParent(this);

            element.OnSelected(() => Window.IsSelected = true);

            var group = element as IIgniteGUIGroup;
            if (group != null)
            {
                LastNestedGroup = group;
            }

            return this;
        }
    }
}