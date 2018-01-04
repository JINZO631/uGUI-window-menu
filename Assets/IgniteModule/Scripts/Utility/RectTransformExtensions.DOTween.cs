using DG.Tweening;
using UnityEngine;

namespace IgniteModule
{
    public static partial class RectTransformExtensions
    {
        public static Tweener DOSizeDeltaX(this RectTransform rectTransform, float endValue, float duration)
        {
            return DOTween.To(() => rectTransform.sizeDelta.x, v => rectTransform.SetSizeDelta(x: v), endValue, duration);
        }

        public static Tweener DOSizeDeltaY(this RectTransform rectTransform, float endValue, float duration)
        {
            return DOTween.To(() => rectTransform.sizeDelta.y, v => rectTransform.SetSizeDelta(y: v), endValue, duration);
        }
    }
}