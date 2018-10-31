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
    }
}