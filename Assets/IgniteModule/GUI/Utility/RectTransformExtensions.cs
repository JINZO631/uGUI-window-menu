using UnityEngine;

namespace IgniteModule.GUICore
{
    public static partial class RectTransformExtensions
    {
        public static void SetSizeDelta(this RectTransform rectTransform, float? x = null, float? y = null)
        {
            var sizeDelta = rectTransform.sizeDelta;
            if (x.HasValue) sizeDelta.x = x.Value;
            if (y.HasValue) sizeDelta.y = y.Value;
            rectTransform.sizeDelta = sizeDelta;
        }

        public static void SetAnchoredPosition(this RectTransform rectTransform, float? x = null, float? y = null)
        {
            var anchoredPosition = rectTransform.anchoredPosition;
            if (x.HasValue) anchoredPosition.x = x.Value;
            if (y.HasValue) anchoredPosition.y = y.Value;
            rectTransform.anchoredPosition = anchoredPosition;
        }
    }
}