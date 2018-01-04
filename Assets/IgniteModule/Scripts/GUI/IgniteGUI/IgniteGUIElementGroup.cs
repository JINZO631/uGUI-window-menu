using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace IgniteModule.UI
{
    public abstract class IgniteGUIElementGroup : UIMonoBehaviour, IIgniteGUIElementGroup
    {
        public string ID { get; protected set; }
        public IgniteWindow Window { get; protected set; }
        public IIgniteGUIGroup Parent { get; protected set; }
        public IIgniteGUIGroup LastNestedGroup { get; protected set; }

        protected virtual Transform Content { get { return Transform; } }
        
        public IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            element.RectTransform.SetParent(Content);
            element.SetSize(Window.Size);
            element.SetTheme(Window.Theme);

            element.OnSelected().Subscribe(_ => Window.Select.Value = true).AddTo(this);

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

        public abstract IObservable<Unit> OnSelected();

        public abstract void SetSize(IIgniteGUISize size);

        public abstract void SetTheme(IIgniteGUITheme theme);
    }
}