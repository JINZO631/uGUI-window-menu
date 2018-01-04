using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace IgniteModule.UI
{
    public class VariadicPanel : Draggable
    {
        [SerializeField] UIMonoBehaviour dragArea;
        [SerializeField] Vector2 minSize = new Vector2(100, 100);
        [SerializeField] Vector2 maxSize = new Vector2(1920, 1080);


        protected override void Start()
        {
            base.Start();

            dragArea
                .OnDragAsObservable()
                .Subscribe(d =>
                {
                    //var anchoredPos = d.delta / 2;
                    //anchoredPos.x = rectTransform.sizeDelta.x <= minSize.x ? 0 : anchoredPos.x;
                    //anchoredPos.y = rectTransform.sizeDelta.y <= minSize.y ? 0 : anchoredPos.y;
                    //rectTransform.anchoredPosition += anchoredPos;

                    var sizeDelta = d.delta;
                    sizeDelta.y = d.delta.y * -1;
                    var localPos = RectTransform.localPosition;
                    RectTransform.sizeDelta += sizeDelta;
                    RectTransform.sizeDelta = IgniteMath.Clamp(RectTransform.sizeDelta, minSize, maxSize);
                    RectTransform.localPosition = localPos;
                });
        }
    }
}