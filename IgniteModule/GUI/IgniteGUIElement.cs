using System;
using UnityEngine.Events;

namespace IgniteModule.GUICore
{
    public abstract class IgniteGUIElement : GUIMonoBehaviour, IIgniteGUIElement
    {
        protected UnityEvent onSelected = new UnityEvent();
        public IgniteWindow Window { get; protected set; }

        public void OnSelected(Action onSelected)
        {
            this.onSelected.AddListener(new UnityAction(onSelected));
        }

        public virtual void SetParent(IIgniteGUIGroup parent)
        {
            Window = parent.Window;

            this.RectTransform.SetParent(parent.Content);
        }
    }
}