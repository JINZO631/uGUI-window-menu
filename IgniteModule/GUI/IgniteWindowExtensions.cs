using UnityEngine;
using IgniteModule.GUICore;

namespace IgniteModule
{
    public static class IgniteWindowExtensions
    {
        public static IgniteWindow SetCenterPos(this IgniteWindow window)
        {
            window.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                var pos = new Vector2(Screen.safeArea.size.x * 0.5f, -Screen.safeArea.size.y * 0.5f);
                pos -= new Vector2(window.RectTransform.sizeDelta.x * 0.5f, window.RectTransform.sizeDelta.x * -0.5f);
                window.RectTransform.anchoredPosition = pos;
            }));

            return window;
        }

        public static IgniteWindow SetLeftTopPos(this IgniteWindow window)
        {
            window.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                window.RectTransform.SetAnchoredPosition(0f, 0f);
            }));

            return window;
        }

        public static IgniteWindow SetRightTopPos(this IgniteWindow window)
        {
            window.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                window.RectTransform.SetAnchoredPosition(Screen.safeArea.size.x - window.RectTransform.sizeDelta.x, 0f);
            }));

            return window;
        }

        public static IgniteWindow SetLeftBottomPos(this IgniteWindow window)
        {
            window.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                window.RectTransform.SetAnchoredPosition(0f, window.RectTransform.sizeDelta.y - Screen.safeArea.size.y);
            }));

            return window;
        }

        public static IgniteWindow SetRightBottomPos(this IgniteWindow window)
        {
            window.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                window.RectTransform.SetAnchoredPosition(Screen.safeArea.size.x - window.RectTransform.sizeDelta.x, window.RectTransform.sizeDelta.y - Screen.safeArea.size.y);
            }));

            return window;
        }

        public static IgniteWindow SetWindowBottom(this IgniteWindow bottomWindow, IgniteWindow baseWindow)
        {
            bottomWindow.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                bottomWindow.RectTransform.SetAnchoredTopPosition(baseWindow.RectTransform.AnchoredBottom());
                bottomWindow.RectTransform.SetAnchoredLeftPosition(baseWindow.RectTransform.AnchoredLeft());
            }));

            return bottomWindow;
        }

        public static IgniteWindow SetWindowTop(this IgniteWindow topWindow, IgniteWindow baseWindow)
        {
            topWindow.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                topWindow.RectTransform.SetAnchoredBottomPosition(baseWindow.RectTransform.AnchoredTop());
                topWindow.RectTransform.SetAnchoredLeftPosition(baseWindow.RectTransform.AnchoredLeft());
            }));

            return topWindow;
        }

        public static IgniteWindow SetWindowLeft(this IgniteWindow leftWindow, IgniteWindow baseWindow)
        {
            leftWindow.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                leftWindow.RectTransform.SetAnchoredRightPosition(baseWindow.RectTransform.AnchoredLeft());
                leftWindow.RectTransform.SetAnchoredTopPosition(baseWindow.RectTransform.AnchoredTop());
            }));

            return leftWindow;
        }

        public static IgniteWindow SetWindowRight(this IgniteWindow rightWindow, IgniteWindow baseWindow)
        {
            rightWindow.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
            {
                rightWindow.RectTransform.SetAnchoredLeftPosition(baseWindow.RectTransform.AnchoredRight());
                rightWindow.RectTransform.SetAnchoredTopPosition(baseWindow.RectTransform.AnchoredTop());
            }));

            return rightWindow;
        }
    }
}