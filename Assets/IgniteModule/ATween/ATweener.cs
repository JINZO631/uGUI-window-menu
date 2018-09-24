using System;
using System.Collections;
using UnityEngine;
using ATweening.Core;

namespace ATweening
{
    public class ATweener<T1, T2> : IATweener where T2 : IEquatable<T2>
    {
        private Action onStart;
        private Action onUpdate;
        private Action onComplete;
        private Action onKill;
        private Action onStepComplete;

        private T1 begin;
        private T1 end;
        private float duration;
        private IEase ease;
        private ITweenPlugin<T1, T2> plugin;
        private Func<T2> getter;
        private Action<T2> setter;
        private bool ignoreTimeScale;
        private bool pause;
        private bool kill;
        private bool isPlaying;
        private bool complete;
        private bool isRelative;
        private bool isBackwise;
        private int loops;
        private LoopType loopType;

        public ATweener(T1 begin, T1 end, float duration, Func<T2> getter, Action<T2> setter, ITweenPlugin<T1, T2> plugin)
        {
            ATween.Initialize();

            this.begin = begin;
            this.end = end;
            this.duration = duration;
            this.getter = getter;
            this.setter = setter;
            this.ease = ATween.DefaultEase;
            this.plugin = plugin;
            this.ignoreTimeScale = ATween.IgnoreTimeScale;
        }

        public bool IsJoined() { return false; }

        public bool IsPlaying() { return isPlaying; }

        public ATweener<T1, T2> OnStart(Action onStart)
        {
            this.onStart = onStart;
            return this;
        }

        IATween IATween.OnStart(Action onStart)
        {
            return OnStart(onStart);
        }

        public ATweener<T1, T2> OnUpdate(Action onUpdate)
        {
            this.onUpdate = onUpdate;
            return this;
        }

        IATween IATween.OnUpdate(Action onUpdate)
        {
            return OnUpdate(onUpdate);
        }

        public ATweener<T1, T2> OnComplete(Action onComplete)
        {
            this.onComplete = onComplete;
            return this;
        }

        IATween IATween.OnComplete(Action onComplete)
        {
            return OnComplete(onComplete);
        }

        public ATweener<T1, T2> OnKill(Action onKill)
        {
            this.onKill = onKill;
            return this;
        }

        IATween IATween.OnKill(Action onKill)
        {
            return OnKill(onKill);
        }

        public ATweener<T1, T2> OnStepComplete(Action onStepComplete)
        {
            this.onStepComplete = onStepComplete;
            return this;
        }

        IATween IATween.OnStepComplete(Action onStepComplete)
        {
            return OnStepComplete(onStepComplete);
        }

        public ATweener<T1, T2> SetEase(IEase ease)
        {
            this.ease = ease;
            return this;
        }

        public ATweener<T1, T2> SetEase(AnimationCurve animationCurve)
        {
            this.ease = new ATweening.Core.AnimationCurveEase(animationCurve);
            return this;
        }

        IATweener IATweener.SetEase(IEase ease)
        {
            return SetEase(ease);
        }

        IATweener IATweener.SetEase(AnimationCurve animationCurve)
        {
            return SetEase(animationCurve);
        }

        public ATweener<T1, T2> SetIgnoreTimeScale(bool ignoreTimeScale)
        {
            this.ignoreTimeScale = ignoreTimeScale;
            return this;
        }

        IATween IATween.SetIgnoreTimeScale(bool ignoreTimeScale)
        {
            return SetIgnoreTimeScale(ignoreTimeScale);
        }

        public ATweener<T1, T2> SetLoops(int loops)
        {
            this.loops = loops;
            return this;
        }

        public ATweener<T1, T2> SetLoops(int loops, LoopType loopType)
        {
            this.loops = loops;
            this.loopType = loopType;
            return this;
        }

        IATween IATween.SetLoops(int loops)
        {
            return SetLoops(loops);
        }

        public IATweener Play()
        {
            if (this.isPlaying)
            {
                return this;
            }

            ATween.Instance.Commit(TweenCoroutine());
            return this;
        }

        IATween IATween.Play()
        {
            return Play();
        }

        public ATweener<T1, T2> Pause()
        {
            this.pause = true;
            return this;
        }

        IATween IATween.Pause()
        {
            return Pause();
        }

        public void Restart()
        {
            this.pause = false;
        }

        public void TogglePause()
        {
            this.pause = !this.pause;
        }

        public void Kill()
        {
            this.kill = true;
            this.isPlaying = false;
        }

        public void Complete()
        {
            this.complete = true;
            this.isPlaying = false;
        }

        public void Flip()
        {
            this.isBackwise = !this.isBackwise;
        }

        public ATweener<T1, T2> SetRelative()
        {
            this.isRelative = true;
            return this;
        }

        public IEnumerator TweenCoroutine()
        {
            Setup();

            if (onStart != null) onStart();

            yield return TweenCoroutineCore();

            while (loops != 0)
            {
                switch (loopType)
                {
                    case LoopType.Restart:
                        break;
                    case LoopType.Yoyo:
                        Flip();
                        break;
                    case LoopType.Incremental:
                        var temp = end;
                        end = this.plugin.IncrementalValue(begin, end);
                        begin = temp;
                        break;
                }

                yield return TweenCoroutineCore();
            }

            if (onComplete != null) onComplete();

            isPlaying = false;
        }

        private IEnumerator TweenCoroutineCore()
        {
            float elapsedTime = InitTime(duration);
            T1 current = default(T1);

            while (TweenCoroutineCondition(elapsedTime, duration))
            {
                if (this.kill)
                {
                    if (onKill != null) onKill();
                    yield break;
                }

                if (this.pause)
                {
                    yield return null;
                    continue;
                }

                if (this.complete)
                {
                    break;
                }

                elapsedTime += UpdateTime();
                var t = ease.Invoke(elapsedTime / duration);
                current = plugin.Lerp(begin, end, t);
                setter(plugin.Convert(current));
                if (onUpdate != null) onUpdate();
                yield return null;
            }

            Snap(plugin.Convert(current));

            if (onStepComplete != null) onStepComplete();

            if (loops > 0)
            {
                --loops;
            }
        }

        private void Setup()
        {
            if (getter != null)
            {
                begin = plugin.Convert(getter());
            }

            if (this.isRelative)
            {
                end = this.plugin.RelativeValue(plugin.Convert(getter()), end);
            }

            this.isPlaying = true;
            this.pause = false;
            this.kill = false;
            this.complete = false;
        }

        private void Snap(T2 current)
        {
            var snapValue = this.isBackwise ? begin : end;
            if (!current.Equals(snapValue))
            {
                setter(plugin.Convert(snapValue));
            }
        }

        private float InitTime(float duration)
        {
            return this.isBackwise ? duration : 0f;
        }

        private bool TweenCoroutineCondition(float elapsedTime, float duration)
        {
            return this.isBackwise ? (0f < elapsedTime) : (elapsedTime < duration);
        }

        private float UpdateTime()
        {
            var timeScale = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            return timeScale * (this.isBackwise ? -1 : +1);
        }
    }
}