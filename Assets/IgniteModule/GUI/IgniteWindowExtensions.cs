using System;
using System.Collections;
using UnityEngine;
using IgniteModule.GUICore;

namespace IgniteModule
{
    public static class IgniteWindowExtensions
    {
        static IEnumerator DelayAction(Action action)
        {
            yield return null;
            action();
        }

        public static IgniteWindow SetCenterPos(this IgniteWindow window)
        {
            window.StartCoroutine(DelayAction(() =>
            {
                var pos = new Vector2(Screen.safeArea.size.x * 0.5f, -Screen.safeArea.size.y * 0.5f);
                pos -= new Vector2(window.RectTransform.sizeDelta.x * 0.5f, window.RectTransform.sizeDelta.x * -0.5f);
                window.RectTransform.anchoredPosition = pos;
            }));

            return window;
        }

        public static IgniteWindow SetLeftTopPos(this IgniteWindow window)
        {
            window.StartCoroutine(DelayAction(() =>
            {
                window.RectTransform.SetAnchoredPosition(0f, 0f);
            }));

            return window;
        }

        public static IgniteWindow SetRightTopPos(this IgniteWindow window)
        {
            window.StartCoroutine(DelayAction(() =>
            {
                window.RectTransform.SetAnchoredPosition(Screen.safeArea.size.x - window.RectTransform.sizeDelta.x, 0f);
            }));

            return window;
        }

        public static IgniteWindow SetLeftBottomPos(this IgniteWindow window)
        {
            window.StartCoroutine(DelayAction(() =>
            {
                window.RectTransform.SetAnchoredPosition(0f, window.RectTransform.sizeDelta.y - Screen.safeArea.size.y);
            }));

            return window;
        }

        public static IgniteWindow SetRightBottomPos(this IgniteWindow window)
        {
            window.StartCoroutine(DelayAction(() =>
            {
                window.RectTransform.SetAnchoredPosition(Screen.safeArea.size.x - window.RectTransform.sizeDelta.x, window.RectTransform.sizeDelta.y - Screen.safeArea.size.y);
            }));

            return window;
        }

        public static IgniteWindow SetWindowBottom(this IgniteWindow bottomWindow, IgniteWindow baseWindow)
        {
            bottomWindow.StartCoroutine(DelayAction(() =>
            {
                bottomWindow.RectTransform.SetAnchoredTopPosition(baseWindow.RectTransform.AnchoredBottom());
                bottomWindow.RectTransform.SetAnchoredLeftPosition(baseWindow.RectTransform.AnchoredLeft());
            }));

            return bottomWindow;
        }

        public static IgniteWindow SetWindowTop(this IgniteWindow topWindow, IgniteWindow baseWindow)
        {
            topWindow.StartCoroutine(DelayAction(() =>
            {
                topWindow.RectTransform.SetAnchoredBottomPosition(baseWindow.RectTransform.AnchoredTop());
                topWindow.RectTransform.SetAnchoredLeftPosition(baseWindow.RectTransform.AnchoredLeft());
            }));

            return topWindow;
        }

        public static IgniteWindow SetWindowLeft(this IgniteWindow leftWindow, IgniteWindow baseWindow)
        {
            leftWindow.StartCoroutine(DelayAction(() =>
            {
                leftWindow.RectTransform.SetAnchoredRightPosition(baseWindow.RectTransform.AnchoredLeft());
                leftWindow.RectTransform.SetAnchoredTopPosition(baseWindow.RectTransform.AnchoredTop());
            }));

            return leftWindow;
        }

        public static IgniteWindow SetWindowRight(this IgniteWindow rightWindow, IgniteWindow baseWindow)
        {
            rightWindow.StartCoroutine(DelayAction(() =>
            {
                rightWindow.RectTransform.SetAnchoredLeftPosition(baseWindow.RectTransform.AnchoredRight());
                rightWindow.RectTransform.SetAnchoredTopPosition(baseWindow.RectTransform.AnchoredTop());
            }));

            return rightWindow;
        }

        public static IgniteWindow CreateWindowBottom(this IgniteWindow window, string name, Vector2? anchoredPosition = null, Vector2? windowSize = null, bool open = true, bool hideCloseButton = false, bool fixedSize = false, bool fixedPosition = false, bool stretch = false)
        {
            var bottomWindow = IgniteWindow.Create(
                name: name,
                anchoredPosition: anchoredPosition,
                windowSize: windowSize,
                open: open,
                hideCloseButton: hideCloseButton,
                fixedSize: fixedSize,
                fixedPosition: fixedPosition,
                stretch: stretch
            );

            bottomWindow.StartCoroutine(DelayAction(() =>
            {
                bottomWindow.RectTransform.SetAnchoredTopPosition(window.RectTransform.AnchoredBottom());
                bottomWindow.RectTransform.SetAnchoredLeftPosition(window.RectTransform.AnchoredLeft());
            }));

            return bottomWindow;
        }
    }
}