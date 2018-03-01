using System;
using UniRx;

namespace IgniteModule.UI
{
    public abstract class IgniteGUIElement : UIMonoBehaviour, IIgniteGUIElement
    {
        readonly Single onInitializeBeforeAsync = new Single();
        readonly Single onInitializeAsync = new Single();
        readonly Single onInitializeAfterAsync = new Single();

        protected override void Start()
        {
            onInitializeBeforeAsync.Done();
            onInitializeAsync.Done();
            onInitializeAfterAsync.Done();
        }

        public virtual void SetParent(IIgniteGUIGroup group)
        {
            this.Window = group.Window;
            this.RectTransform.SetParent(group.Content);
        }

        public IObservable<Unit> OnInitializeBeforeAsync()
        {
            return onInitializeBeforeAsync;
        }

        public IObservable<Unit> OnInitializeAsync()
        {
            return onInitializeAsync;
        }

        public IObservable<Unit> OnInitializeAfterAsync()
        {
            return onInitializeAfterAsync;
        }

        public string ID { get; protected set; }

        public IgniteWindow Window { get; protected set; }

        public abstract IObservable<Unit> OnSelected();

        public abstract void SetSize(IIgniteGUISize size);

        public abstract void SetTheme(IIgniteGUITheme theme);
    }
}