using UnityEngine;
using UnityEngine.UI;

namespace ATweening
{
    public static class ShortcutExtensions
    {
        public static ATweener<Vector3, Vector3> DoMove(this Transform transform, Vector3 endValue, float duration)
        {
            return ATween.To(() => transform.position, v => transform.position = v, endValue, duration);
        }

        public static ATweener<float, float> DoMoveX(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.position.x, v =>
            {
                var pos = transform.position;
                pos.x = v;
                transform.position = pos;
            }, endValue, duration);
        }

        public static ATweener<float, float> DoMoveY(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.position.y, v =>
            {
                var pos = transform.position;
                pos.y = v;
                transform.position = pos;
            }, endValue, duration);
        }

        public static ATweener<float, float> DoMoveZ(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.position.z, v =>
            {
                var pos = transform.position;
                pos.z = v;
                transform.position = pos;
            }, endValue, duration);
        }

        public static ATweener<Vector3, Vector3> DoLocalMove(this Transform transform, Vector3 endValue, float duration)
        {
            return ATween.To(() => transform.localPosition, v => transform.localPosition = v, endValue, duration);
        }

        public static ATweener<float, float> DoLocalMoveX(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.localPosition.x, v =>
            {
                var pos = transform.localPosition;
                pos.x = v;
                transform.localPosition = pos;
            }, endValue, duration);
        }

        public static ATweener<float, float> DoLocalMoveY(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.localPosition.y, v =>
            {
                var pos = transform.localPosition;
                pos.y = v;
                transform.localPosition = pos;
            }, endValue, duration);
        }

        public static ATweener<float, float> DoLocalMoveZ(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.localPosition.z, v =>
            {
                var pos = transform.localPosition;
                pos.z = v;
                transform.localPosition = pos;
            }, endValue, duration);
        }

        public static ATweener<Vector3, Quaternion> DoRotate(this Transform transform, Vector3 endValue, float duration)
        {
            return ATween.To(() => transform.rotation, v => transform.rotation = v, endValue, duration);
        }

        public static ATweener<Vector3, Quaternion> DoLocalRotate(this Transform transform, Vector3 endValue, float duration)
        {
            return ATween.To(() => transform.localRotation, v => transform.localRotation = v, endValue, duration);
        }

        public static ATweener<Vector3, Vector3> DoScale(this Transform transform, Vector3 endValue, float duration)
        {
            return ATween.To(() => transform.localScale, v => transform.localScale = v, endValue, duration);
        }

        public static ATweener<Vector3, Vector3> DoScale(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.localScale, v => transform.localScale = v, new Vector3(endValue, endValue, endValue), duration);
        }

        public static ATweener<float, float> DoScaleX(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.localScale.x, v =>
            {
                var scale = transform.localScale;
                scale.x = v;
                transform.localScale = scale;
            }, endValue, duration);
        }

        public static ATweener<float, float> DoScaleY(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.localScale.y, v =>
            {
                var scale = transform.localScale;
                scale.y = v;
                transform.localScale = scale;
            }, endValue, duration);
        }

        public static ATweener<float, float> DoScaleZ(this Transform transform, float endValue, float duration)
        {
            return ATween.To(() => transform.localScale.z, v =>
            {
                var scale = transform.localScale;
                scale.z = v;
                transform.localScale = scale;
            }, endValue, duration);
        }

        public static ATweener<Vector2, Vector2> DoAnchorPos(this RectTransform rectTransform, Vector2 endValue, float duration)
        {
            return ATween.To(() => rectTransform.anchoredPosition, v => rectTransform.anchoredPosition = v, endValue, duration);
        }

        public static ATweener<float, float> DoAnchorPosX(this RectTransform rectTransform, float endValue, float duration)
        {
            return ATween.To(() => rectTransform.anchoredPosition.x, v =>
            {
                var vec = rectTransform.anchoredPosition;
                vec.x = v;
                rectTransform.anchoredPosition = vec;
            }, endValue, duration);
        }

        public static ATweener<float, float> DoAnchorPosY(this RectTransform rectTransform, float endValue, float duration)
        {
            return ATween.To(() => rectTransform.anchoredPosition.y, v =>
            {
                var vec = rectTransform.anchoredPosition;
                vec.y = v;
                rectTransform.anchoredPosition = vec;
            }, endValue, duration);
        }

        public static ATweener<Vector2, Vector2> DoSizeDelta(this RectTransform rectTransform, Vector2 endValue, float duration)
        {
            return ATween.To(() => rectTransform.sizeDelta, v => rectTransform.sizeDelta = v, endValue, duration);
        }

        public static ATweener<float, float> DoSizeDeltaX(this RectTransform rectTransform, float endValue, float duration)
        {
            return ATween.To(() => rectTransform.sizeDelta.x, v =>
            {
                var size = rectTransform.sizeDelta;
                size.x = v;
                rectTransform.sizeDelta = size;
            }, endValue, duration);
        }

        public static ATweener<float, float> DoSizeDeltaY(this RectTransform rectTransform, float endValue, float duration)
        {
            return ATween.To(() => rectTransform.sizeDelta.y, v =>
            {
                var size = rectTransform.sizeDelta;
                size.y = v;
                rectTransform.sizeDelta = size;
            }, endValue, duration);
        }

        public static ATweener<Color, Color> DoColor(this Graphic graphic, Color endValue, float duration)
        {
            return ATween.To(() => graphic.color, v => graphic.color = v, endValue, duration);
        }

        public static ATweener<float, float> DoFade(this Graphic graphic, float endValue, float duration)
        {
            return ATween.To(() => graphic.color.a, v =>
            {
                var c = graphic.color;
                c.a = v;
                graphic.color = c;
            }, endValue, duration);
        }
    }
}