using System.Collections;
using UnityEngine;
using UniRx;
using System;

namespace IgniteModule
{
    public static class TweenUtil
    {
        static readonly Func<float, float> Ease = t => OutQuint(t);
        static float OutQuint(float t)
        {
            return Mathf.Pow(t - 1f, 5f) + 1f;
        }

        public static IObservable<Unit> Tween(RectTransform sizeRect, Vector2 endSize, RectTransform rotateRect, Quaternion endRotate, float duration)
        {

            return Observable.FromMicroCoroutine<Unit>((observer) => TweenCoroutine(observer, sizeRect, endSize, rotateRect, endRotate, duration, Ease));
        }

        static IEnumerator TweenCoroutine(IObserver<Unit> observer, RectTransform sizeRect, Vector2 endSize, RectTransform rotateRect, Quaternion endRotate, float duration, Func<float, float> ease)
        {
            var beginSize = sizeRect.sizeDelta;
            var beginRotate = rotateRect.localRotation;

            float time = 0f;
            while (time <= duration)
            {
                var t = time / duration;
                t = ease(t);
                sizeRect.sizeDelta = Vector2.Lerp(beginSize, endSize, t);
                rotateRect.localRotation = Quaternion.Lerp(beginRotate, endRotate, t);
                observer.OnNext(Unit.Default);
                yield return null;
                time += Time.deltaTime;
            }

            observer.OnNext(Unit.Default);
            observer.OnCompleted();
        }
    }
}