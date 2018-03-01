using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace IgniteModule.UI
{
    public abstract class IgniteGUIElementGroup : IgniteGUIElement, IIgniteGUIElementGroup
    {
        public IIgniteGUIGroup Parent { get; protected set; }
        public IIgniteGUIGroup LastNestedGroup { get; protected set; }

        public virtual Transform Content { get { return Transform; } }

        public override void SetParent(IIgniteGUIGroup parent)
        {
            base.SetParent(parent);
            this.Parent = parent;
        }
        
        public virtual IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            element.SetParent(this);

            element.OnInitializeBeforeAsync()
            .SubscribeWithState2(element, this, (_, e, g) =>
            {
                e.SetSize(g.Window.Size);
                e.SetTheme(g.Window.Theme);
                e.OnSelected().SubscribeWithState(g.Window, (__, window) => window.Select.Value = true).AddTo(g);
            });

            var group = element as IIgniteGUIGroup;
            if (group != null)
            {
                LastNestedGroup = group;
            }

            return this;
        }

        public IIgniteGUIElement FindChild(string id)
        {
            return Transform.GetComponentsInChildren<IIgniteGUIElement>().FirstOrDefault(c => c.ID == id);
        }

        public TElement FindChild<TElement>(string id) where TElement : Component, IIgniteGUIElement
        {
            return Transform.GetComponentsInChildren<TElement>().FirstOrDefault(c => c.ID == id);
        }
    }
}