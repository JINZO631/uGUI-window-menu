using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System;

namespace IgniteModule.UI
{
    public class VariableSizePanel : UIMonoBehaviour
    {
        [SerializeField] UIMonoBehaviour dragArea;
        [SerializeField] Vector2 minSize = new Vector2(100, 100);
        [SerializeField] Vector2 maxSize = new Vector2(1920, 1080);

        public IObservable<Unit> OnSizeChanged()
        {
            return dragArea.OnEndDragAsObservable().AsUnitObservable();
        }

        protected override void Start()
        {
            base.Start();

            dragArea
                .OnDragAsObservable()
                .RepeatUntilDestroy(this)
                .Subscribe(d =>
                {
                    var sizeDelta = d.delta;
                    sizeDelta.y = d.delta.y * -1;
                    RectTransform.sizeDelta += sizeDelta;
                    RectTransform.sizeDelta = IgniteMath.Clamp(RectTransform.sizeDelta, minSize, maxSize);
                });
        }
    }
}
