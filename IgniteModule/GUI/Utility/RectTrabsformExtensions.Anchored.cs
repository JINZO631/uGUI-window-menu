using UnityEngine;

namespace IgniteModule.GUICore
{
    public static partial class RectTransformExtensions
    {
        /// <summary> SizeDeltaのxを計算 </summary>
        public static float CalcSizeDeltaX(this RectTransform rectTransform, float anchorMinX, float anchorMaxX)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentWidth = parent.rect.width;
            var width = rectTransform.rect.width;
            return width - (parentWidth * (anchorMaxX - anchorMinX));
        }

        /// <summary> SizeDeltaのyを計算 </summary>
        public static float CalcSizeDeltaY(this RectTransform rectTransform, float anchorMinY, float anchorMaxY)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentHeight = parent.rect.height;
            var height = rectTransform.rect.height;
            return height - (parentHeight * (anchorMaxY - anchorMinY));
        }

        /// <summary> SizeDeltaを計算 </summary>
        public static Vector2 CalcSizeDelta(this RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentRect = parent.rect;
            var rect = rectTransform.rect;

            return new Vector2(rect.width - (parentRect.width * (anchorMax.x - anchorMin.x)), rect.height - (parentRect.height * (anchorMax.y - anchorMin.y)));
        }

        /// <summary> AnchoredPositionのxを計算 </summary>
        public static float CalcAnchoredPositionX(this RectTransform rectTransform, float anchorMinX, float anchorMaxX, float pivotX)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentRect = parent.rect;
            var anchorWidth = ((anchorMinX * parentRect.width) + (anchorMaxX * parentRect.width));
            var anchorCenter = parent.Left() + (anchorWidth * 0.5f);

            return rectTransform.position.x - anchorCenter;
        }

        /// <summary> AnchoredPositionのyを計算 </summary>
        public static float CalcAnchoredPositionY(this RectTransform rectTransform, float anchorMinY, float anchorMaxY, float pivotY)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentRect = parent.rect;
            var anchorHeight = ((anchorMinY * parentRect.height) + (anchorMaxY * parentRect.height));
            var anchorCenter = parent.Bottom() + (anchorHeight * 0.5f);
            return rectTransform.position.y - anchorCenter;
        }

        /// <summary> AnchoredPositionを計算 </summary>
        public static Vector2 CalcAnchoredPosition(this RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentRect = parent.rect;
            var anchorWidth = ((anchorMin.x * parentRect.width) + (anchorMax.x * parentRect.width));
            var anchorHeight = ((anchorMin.y * parentRect.height) + (anchorMax.y * parentRect.height));
            var anchorCenter = new Vector2(parent.Left() + (anchorWidth * 0.5f), parent.Bottom() + (anchorHeight * 0.5f));

            return (Vector2)rectTransform.position - anchorCenter;
        }

        /// <summary> RectTransformの上辺座標を計算 </summary>
        public static float Top(this RectTransform rectTransform)
        {
            return rectTransform.position.y + (rectTransform.rect.height * (1 - rectTransform.pivot.y));
        }

        /// <summary> RectTransformの下辺座標を計算 </summary>
        public static float Bottom(this RectTransform rectTransform)
        {
            return rectTransform.position.y - (rectTransform.rect.height * (rectTransform.pivot.y));
        }

        /// <summary> RectTransformの左辺座標を計算 </summary>
        public static float Left(this RectTransform rectTransform)
        {
            return rectTransform.position.x - (rectTransform.rect.width * (rectTransform.pivot.x));
        }

        /// <summary> RectTransformの右辺座標を計算 </summary>
        public static float Right(this RectTransform rectTransform)
        {
            return rectTransform.position.x + (rectTransform.rect.width * (1 - rectTransform.pivot.x));
        }

        /// <summary> 上辺のアンカー座標を計算 </summary>
        public static float AnchoredTop(this RectTransform rectTransform)
        {
            return rectTransform.anchoredPosition.y + (rectTransform.sizeDelta.y * (1 - rectTransform.pivot.y));
        }

        /// <summary> 下辺のアンカー座標を計算 </summary>
        public static float AnchoredBottom(this RectTransform rectTransform)
        {
            return rectTransform.anchoredPosition.y - (rectTransform.sizeDelta.y * rectTransform.pivot.y);
        }

        /// <summary> 左辺のアンカー座標を計算 </summary>
        public static float AnchoredLeft(this RectTransform rectTransform)
        {
            return rectTransform.anchoredPosition.x - (rectTransform.sizeDelta.x * rectTransform.pivot.x);
        }

        /// <summary> 右辺のアンカー座標を計算 </summary>
        public static float AnchoredRight(this RectTransform rectTransform)
        {
            return rectTransform.anchoredPosition.x + (rectTransform.sizeDelta.x * (1 - rectTransform.pivot.x));
        }

        /// <summary> 親RectTransformの上辺位置までのアンカー座標を計算 </summary>
        public static float AnchoredParentTop(this RectTransform rectTransform)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentRect = parent.rect;
            return parentRect.height - (parentRect.height * rectTransform.anchorMin.y * 0.5f) - (parentRect.height * rectTransform.anchorMax.y * 0.5f);
        }

        /// <summary> 親RectTransformの下辺位置までのアンカー座標を計算 </summary>
        public static float AnchoredParentBottom(this RectTransform rectTransform)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentRect = parent.rect;
            return -(parentRect.height * rectTransform.anchorMin.y * 0.5f) - (parentRect.height * rectTransform.anchorMax.y * 0.5f);
        }

        /// <summary> 親RectTransformの左辺位置までのアンカー座標を計算 </summary>
        public static float AnchoredParentLeft(this RectTransform rectTransform)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentRect = parent.rect;
            return -(parentRect.width * rectTransform.anchorMin.x * 0.5f) - (parentRect.width * rectTransform.anchorMax.x * 0.5f);
        }

        /// <summary> 親RectTransformの右辺位置までのアンカー座標を計算 </summary>
        public static float AnchoredParentRight(this RectTransform rectTransform)
        {
            var parent = rectTransform.parent as RectTransform;
            var parentRect = parent.rect;
            return parentRect.width - (parentRect.width * rectTransform.anchorMin.x * 0.5f) - (parentRect.width * rectTransform.anchorMax.x * 0.5f);
        }

        /// <summary> 上辺座標指定でのRectTransformの座標設定 </summary>
        public static void SetTopPosition(this RectTransform rectTransform, float topPosition)
        {
            var diff = rectTransform.Top() - rectTransform.position.y;
            var pos = new Vector2(rectTransform.position.x, topPosition - diff);
            rectTransform.position = pos;
        }

        /// <summary> 下辺座標指定でのRectTransformの座標設定 </summary>
        public static void SetBottomPosition(this RectTransform rectTransform, float bottomPosition)
        {
            var diff = rectTransform.position.y - rectTransform.Bottom();
            var pos = new Vector2(rectTransform.position.x, bottomPosition + diff);
            rectTransform.position = pos;
        }

        /// <summary> 左辺座標指定でのRectTransformの座標設定 </summary>
        public static void SetLeftPosition(this RectTransform rectTransform, float leftPosition)
        {
            var diff = rectTransform.position.x - rectTransform.Left();
            var pos = new Vector2(leftPosition + diff, rectTransform.position.y);
            rectTransform.position = pos;
        }

        /// <summary> 右辺座標指定でのRectTransformの座標設定 </summary>
        public static void SetRightPosition(this RectTransform rectTransform, float rightPosition)
        {
            var diff = rectTransform.Right() - rectTransform.position.x;
            var pos = new Vector2(rightPosition - diff, rectTransform.position.y);
            rectTransform.position = pos;
        }

        /// <summary> 上辺アンカー座標指定でのRectTransformの座標設定 </summary>
        public static void SetAnchoredTopPosition(this RectTransform rectTransform, float anchoredTopPosition)
        {
            var diff = rectTransform.AnchoredTop() - rectTransform.anchoredPosition.y;
            var pos = new Vector2(rectTransform.anchoredPosition.x, anchoredTopPosition - diff);
            rectTransform.anchoredPosition = pos;
        }

        /// <summary> 下辺アンカー座標指定でのRectTransformの座標設定 </summary>
        public static void SetAnchoredBottomPosition(this RectTransform rectTransform, float anchoredBottomPosition)
        {
            var diff = rectTransform.anchoredPosition.y - rectTransform.AnchoredBottom();
            var pos = new Vector2(rectTransform.anchoredPosition.x, anchoredBottomPosition + diff);
            rectTransform.anchoredPosition = pos;
        }

        /// <summary> 左辺アンカー座標指定でのRectTransformの座標設定 </summary>
        public static void SetAnchoredLeftPosition(this RectTransform rectTransform, float anchoredLeftPosition)
        {
            var diff = rectTransform.anchoredPosition.x - rectTransform.AnchoredLeft();
            var pos = new Vector2(anchoredLeftPosition + diff, rectTransform.anchoredPosition.y);
            rectTransform.anchoredPosition = pos;
        }

        /// <summary> 右辺アンカー座標指定でのRectTransformの座標設定 </summary>
        public static void SetAnchoredRightPosition(this RectTransform rectTransform, float anchoredRightPosition)
        {
            var diff = rectTransform.AnchoredRight() - rectTransform.anchoredPosition.x;
            var pos = new Vector2(anchoredRightPosition - diff, rectTransform.anchoredPosition.y);
            rectTransform.anchoredPosition = pos;
        }
    }
}