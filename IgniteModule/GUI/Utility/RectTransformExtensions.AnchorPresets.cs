using UnityEngine;
using System;

namespace IgniteModule
{
    public enum AnchorPresets
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,

        StretchTop,
        StretchMiddle,
        StretchBottom,

        StretchLeft,
        StretchCenter,
        StretchRight,

        StretchAll,

        None
    }

    public static partial class RectTransformExtensions
    {
        struct Anchor
        {
            public Vector2 min;
            public Vector2 max;
            public Anchor(Vector2 min, Vector2 max)
            {
                this.min = min;
                this.max = max;
            }

            public bool Equals(RectTransform rectTransform)
            {
                return (rectTransform.anchorMin == min && rectTransform.anchorMax == max);
            }
        }

        public static void SetAnchorPreset(this RectTransform rectTransform, AnchorPresets preset, bool setPivot = false, bool setPos = false)
        {
            var anchor = ToAnchor(preset);
            rectTransform.anchorMin = anchor.min;
            rectTransform.anchorMax = anchor.max;

            if (setPivot)
            {
                rectTransform.pivot = ToPivot(preset);
            }

            if (setPos)
            {
                rectTransform.anchoredPosition = ToAnchoredPosition(preset, rectTransform);
                rectTransform.sizeDelta = ToSizeDelta(preset, rectTransform);
            }

            if (setPivot && setPos)
            {
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }

        public static AnchorPresets GetAnchorPresets(this RectTransform rectTransform)
        {
            foreach (AnchorPresets preset in Enum.GetValues(typeof(AnchorPresets)))
            {
                if (ToAnchor(preset).Equals(rectTransform))
                {
                    return preset;
                }
            }

            return AnchorPresets.None;
        }

        private static Anchor ToAnchor(AnchorPresets preset)
        {
            switch (preset)
            {
                case AnchorPresets.TopLeft: return AnchorMinMax(0.0f, 1.0f, 0.0f, 1.0f);
                case AnchorPresets.TopCenter: return AnchorMinMax(0.5f, 1.0f, 0.5f, 1.0f);
                case AnchorPresets.TopRight: return AnchorMinMax(1.0f, 1.0f, 1.0f, 1.0f);
                case AnchorPresets.MiddleLeft: return AnchorMinMax(0.0f, 0.5f, 0.0f, 0.5f);
                case AnchorPresets.MiddleCenter: return AnchorMinMax(0.5f, 0.5f, 0.5f, 0.5f);
                case AnchorPresets.MiddleRight: return AnchorMinMax(1.0f, 0.5f, 1.0f, 0.5f);
                case AnchorPresets.BottomLeft: return AnchorMinMax(0.0f, 0.0f, 0.0f, 0.0f);
                case AnchorPresets.BottomCenter: return AnchorMinMax(0.5f, 0.0f, 0.5f, 0.0f);
                case AnchorPresets.BottomRight: return AnchorMinMax(1.0f, 0.0f, 1.0f, 0.0f);
                case AnchorPresets.StretchTop: return AnchorMinMax(0.0f, 1.0f, 1.0f, 1.0f);
                case AnchorPresets.StretchMiddle: return AnchorMinMax(0.0f, 0.5f, 1.0f, 0.5f);
                case AnchorPresets.StretchBottom: return AnchorMinMax(0.0f, 0.0f, 1.0f, 0.0f);
                case AnchorPresets.StretchLeft: return AnchorMinMax(0.0f, 0.0f, 0.0f, 1.0f);
                case AnchorPresets.StretchCenter: return AnchorMinMax(0.5f, 0.0f, 0.5f, 1.0f);
                case AnchorPresets.StretchRight: return AnchorMinMax(1.0f, 0.0f, 1.0f, 1.0f);
                case AnchorPresets.StretchAll: return AnchorMinMax(0.0f, 0.0f, 1.0f, 1.0f);
            }

            return AnchorMinMax(0.5f, 0.5f, 0.5f, 0.5f);
        }

        private static Anchor AnchorMinMax(float minX, float minY, float maxX, float maxY)
        {
            return new Anchor(new Vector2(minX, minY), new Vector2(maxX, maxY));
        }

        private static Vector2 ToPivot(AnchorPresets presets)
        {
            switch (presets)
            {
                case AnchorPresets.TopLeft: return new Vector2(0.0f, 1.0f);
                case AnchorPresets.TopCenter: return new Vector2(0.5f, 1.0f);
                case AnchorPresets.TopRight: return new Vector2(1.0f, 1.0f);
                case AnchorPresets.MiddleLeft: return new Vector2(0.0f, 0.5f);
                case AnchorPresets.MiddleCenter: return new Vector2(0.5f, 0.5f);
                case AnchorPresets.MiddleRight: return new Vector2(1.0f, 0.5f);
                case AnchorPresets.BottomLeft: return new Vector2(0.0f, 0.0f);
                case AnchorPresets.BottomCenter: return new Vector2(0.5f, 0.0f);
                case AnchorPresets.BottomRight: return new Vector2(1.0f, 0.0f);
                case AnchorPresets.StretchTop: return new Vector2(0.5f, 1.0f);
                case AnchorPresets.StretchMiddle: return new Vector2(0.5f, 0.5f);
                case AnchorPresets.StretchBottom: return new Vector2(0.5f, 0.0f);
                case AnchorPresets.StretchLeft: return new Vector2(0.0f, 0.5f);
                case AnchorPresets.StretchCenter: return new Vector2(0.5f, 0.5f);
                case AnchorPresets.StretchRight: return new Vector2(1.0f, 0.5f);
                case AnchorPresets.StretchAll: return new Vector2(0.5f, 0.5f);
            }

            return new Vector2(0.5f, 0.5f);
        }

        private static Vector2 ToAnchoredPosition(AnchorPresets preset, RectTransform rt)
        {
            switch (preset)
            {
                case AnchorPresets.TopLeft: return new Vector2(rt.sizeDelta.x * rt.pivot.x, -rt.sizeDelta.y + rt.pivot.y * rt.sizeDelta.y);
                case AnchorPresets.TopCenter: return new Vector2(rt.sizeDelta.x * (rt.pivot.x - 0.5f), -rt.sizeDelta.y + rt.pivot.y * rt.sizeDelta.y);
                case AnchorPresets.TopRight: return new Vector2(-rt.sizeDelta.x + rt.pivot.x * rt.sizeDelta.x, -rt.sizeDelta.y + rt.pivot.y * rt.sizeDelta.y);

                case AnchorPresets.MiddleLeft: return new Vector2(rt.sizeDelta.x * +rt.pivot.x, -rt.sizeDelta.y * 0.5f + rt.pivot.y * rt.sizeDelta.y);
                case AnchorPresets.MiddleCenter: return new Vector2(rt.sizeDelta.x * (rt.pivot.x - 0.5f), -rt.sizeDelta.y * 0.5f + rt.pivot.y * rt.sizeDelta.y);
                case AnchorPresets.MiddleRight: return new Vector2(-rt.sizeDelta.x + rt.pivot.x * rt.sizeDelta.x, -rt.sizeDelta.y * 0.5f + rt.pivot.y * rt.sizeDelta.y);

                case AnchorPresets.BottomLeft: return new Vector2(rt.sizeDelta.x * +rt.pivot.x, rt.sizeDelta.y * rt.pivot.y);
                case AnchorPresets.BottomCenter: return new Vector2(rt.sizeDelta.x * (rt.pivot.x - 0.5f), rt.sizeDelta.y * rt.pivot.y);
                case AnchorPresets.BottomRight: return new Vector2(-rt.sizeDelta.x + rt.pivot.x * rt.sizeDelta.x, rt.sizeDelta.y * rt.pivot.y);

                case AnchorPresets.StretchTop: return new Vector2(0.0f, rt.sizeDelta.y * -rt.sizeDelta.y + rt.pivot.y * rt.sizeDelta.y);
                case AnchorPresets.StretchMiddle: return new Vector2(0.0f, -rt.sizeDelta.y * 0.5f + rt.pivot.y * rt.sizeDelta.y);
                case AnchorPresets.StretchBottom: return new Vector2(0.0f, rt.sizeDelta.y * rt.pivot.y);

                case AnchorPresets.StretchLeft: return new Vector2(rt.sizeDelta.x * rt.pivot.x, 0.0f);
                case AnchorPresets.StretchCenter: return new Vector2(rt.sizeDelta.x * (rt.pivot.x - 0.5f), 0.0f);
                case AnchorPresets.StretchRight: return new Vector2(-rt.sizeDelta.x + rt.pivot.x * rt.sizeDelta.x, 0.0f);

                case AnchorPresets.StretchAll: return Vector2.zero;
            }

            return Vector2.zero;
        }

        private static Vector2 ToSizeDelta(AnchorPresets preset, RectTransform rt)
        {
            switch (preset)
            {
                case AnchorPresets.StretchTop:
                case AnchorPresets.StretchMiddle:
                case AnchorPresets.StretchBottom:
                    return new Vector2(0.0f, rt.sizeDelta.y);
                case AnchorPresets.StretchLeft:
                case AnchorPresets.StretchCenter:
                case AnchorPresets.StretchRight:
                    return new Vector2(rt.sizeDelta.x, 0.0f);
                case AnchorPresets.StretchAll:
                    return Vector2.zero;
            }

            return rt.sizeDelta;
        }
    }
}