/*
    This is free and unencumbered software released into the public domain.

    Anyone is free to copy, modify, publish, use, compile, sell, or
    distribute this software, either in source code form or as a compiled
    binary, for any purpose, commercial or non-commercial, and by any
    means.

    In jurisdictions that recognize copyright laws, the author or authors
    of this software dedicate any and all copyright interest in the
    software to the public domain. We make this dedication for the benefit
    of the public at large and to the detriment of our heirs and
    successors. We intend this dedication to be an overt act of
    relinquishment in perpetuity of all present and future rights to this
    software under copyright law.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE.

    For more information, please refer to <http://unlicense.org>
 */

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ATweening.Core;

namespace ATweening
{
    [DisallowMultipleComponent]
    public sealed class ATween : MonoBehaviour
    {
        /// <summary>
        /// デフォルトのイージング設定
        /// </summary>
        public static IEase DefaultEase = Ease.OutQuint;

        /// <summary>
        /// すべてのATweenでタイムスケールを無視するか
        /// </summary>
        public static bool IgnoreTimeScale = false;

        /// <summary>
        /// デフォルトの自動Tween再生設定
        /// </summary>
        public static bool DefaultAutoPlay = true;

        private static ATween instance;
        public static ATween Instance
        {
            get
            {
                Initialize();
                return instance;
            }
        }

        private static bool initialized = false;
        private static object lockObj = new object();
        private static List<IEnumerator> coroutines = new List<IEnumerator>();
        private static List<IATweener> tweeners = new List<IATweener>();
        private static List<Sequence> sequences = new List<Sequence>();

        public static void Initialize()
        {
            if (initialized) return;

            instance = new GameObject("[ATween]").AddComponent<ATween>();

            initialized = true;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (0 < tweeners.Count)
            {
                var tweenerList = new List<IATweener>(tweeners);
                tweeners.Clear();

                foreach (var tweener in tweenerList)
                {
                    if (DefaultAutoPlay)
                    {
                        StartCoroutine(tweener.TweenCoroutine());
                    }
                }
            }

            if (0 < sequences.Count)
            {
                var sequenceList = new List<Sequence>(sequences);
                sequences.Clear();
                foreach (var sequence in sequenceList)
                {
                    if (DefaultAutoPlay)
                    {
                        StartCoroutine(sequence.TweenCoroutine());
                    }
                }
            }

            if (0 < coroutines.Count)
            {
                lock (lockObj)
                {
                    var commitingList = new List<IEnumerator>(coroutines);
                    coroutines.Clear();

                    foreach (var coroutine in commitingList)
                    {
                        StartCoroutine(coroutine);
                    }
                }
            }
        }

        public void Commit(IEnumerator ienumerator)
        {
            lock (lockObj)
            {
                coroutines.Add(ienumerator);
            }
        }

        public static Sequence Sequence()
        {
            var sequence = new Sequence();

            return sequence;
        }

        public static ATweener<float, float> To(Func<float> getter, Action<float> setter, float endValue, float duration)
        {
            return TweenCore<float, float>(endValue, duration, getter, setter, TweenPlugin.Float);
        }

        public static ATweener<double, double> To(Func<double> getter, Action<double> setter, double endValue, float duration)
        {
            return TweenCore<double, double>(endValue, duration, getter, setter, TweenPlugin.Double);
        }

        public static ATweener<int, int> To(Func<int> getter, Action<int> setter, int endValue, float duration)
        {
            return TweenCore<int, int>(endValue, duration, getter, setter, TweenPlugin.Int);
        }

        public static ATweener<uint, uint> To(Func<uint> getter, Action<uint> setter, uint endValue, float duration)
        {
            return TweenCore<uint, uint>(endValue, duration, getter, setter, TweenPlugin.Uint);
        }

        public static ATweener<long, long> To(Func<long> getter, Action<long> setter, long endValue, float duration)
        {
            return TweenCore<long, long>(endValue, duration, getter, setter, TweenPlugin.Long);
        }

        public static ATweener<ulong, ulong> To(Func<ulong> getter, Action<ulong> setter, ulong endValue, float duration)
        {
            return TweenCore<ulong, ulong>(endValue, duration, getter, setter, TweenPlugin.Ulong);
        }

        public static ATweener<Vector2, Vector2> To(Func<Vector2> getter, Action<Vector2> setter, Vector2 endValue, float duration)
        {
            return TweenCore<Vector2, Vector2>(endValue, duration, getter, setter, TweenPlugin.Vector2);
        }

        public static ATweener<Vector3, Vector3> To(Func<Vector3> getter, Action<Vector3> setter, Vector3 endValue, float duration)
        {
            return TweenCore<Vector3, Vector3>(endValue, duration, getter, setter, TweenPlugin.Vector3);
        }

        public static ATweener<Vector4, Vector4> To(Func<Vector4> getter, Action<Vector4> setter, Vector4 endValue, float duration)
        {
            return TweenCore<Vector4, Vector4>(endValue, duration, getter, setter, TweenPlugin.Vector4);
        }

        public static ATweener<Color, Color> To(Func<Color> getter, Action<Color> setter, Color endValue, float duration)
        {
            return TweenCore<Color, Color>(endValue, duration, getter, setter, TweenPlugin.Color);
        }

        public static ATweener<Vector3, Quaternion> To(Func<Quaternion> getter, Action<Quaternion> setter, Vector3 endValue, float duration)
        {
            return TweenCore<Vector3, Quaternion>(endValue, duration, getter, setter, TweenPlugin.Quaternion);
        }

        private static ATweener<T1, T2> TweenCore<T1, T2>(T1 endValue, float duration, Func<T2> getter, Action<T2> setter, ITweenPlugin<T1, T2> plugin)
        {
            var tweener = new ATweener<T1, T2>(default(T1), endValue, duration, getter, setter, plugin);
            tweeners.Add(tweener);
            return tweener;
        }
    }

    public interface IATween
    {
        bool IsPlaying();
        IATween OnStart(Action onStart);
        IATween OnUpdate(Action onUpdate);
        IATween OnComplete(Action onComplete);
        IATween OnKill(Action onKill);
        IATween OnStepComplete(Action onStepComplete);
        IATween SetIgnoreTimeScale(bool ignoreTimeScale);
        IATween SetLoops(int loops);
        IATween Play();
        IATween Pause();
        void Restart();
        void TogglePause();
        void Kill();
        void Complete();
        void Flip();
        IEnumerator TweenCoroutine();
    }

    public interface IATweener : IATween
    {
        IATweener SetEase(IEase ease);
        IATweener SetEase(AnimationCurve ease);
    }

    public class ATweener<T1, T2> : IATweener
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
            if (!plugin.Equals(snapValue, current))
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

    public enum LoopType
    {
        Restart,
        Yoyo,
        Incremental
    }

    public class Sequence : IATween
    {
        private LinkedList<ISequenceable> queue = new LinkedList<ISequenceable>();

        private bool ignoreTimeScale;
        private Action onStart;
        private Action onUpdate;
        private Action onComplete;
        private Action onKill;
        private Action onStepComplete;
        private bool pause;
        private bool kill;
        private bool isPlaying;
        private bool complete;
        private int loops;
        private bool isBackwise;

        public Sequence()
        {
            ignoreTimeScale = ATween.IgnoreTimeScale;
        }

        public bool IsJoined()
        {
            return false;
        }

        public bool IsPlaying()
        {
            return this.isPlaying;
        }

        public Sequence OnStart(Action onStart)
        {
            this.onStart = onStart;
            return this;
        }

        IATween IATween.OnStart(Action onStart)
        {
            return OnStart(onStart);
        }

        public Sequence OnUpdate(Action onUpdate)
        {
            this.onUpdate = onUpdate;
            return this;
        }

        IATween IATween.OnUpdate(Action onUpdate)
        {
            return OnUpdate(onUpdate);
        }

        public Sequence OnComplete(Action onComplete)
        {
            this.onComplete = onComplete;
            return this;
        }

        IATween IATween.OnComplete(Action onComplete)
        {
            return OnComplete(onComplete);
        }

        public Sequence OnKill(Action onKill)
        {
            this.onKill = onKill;
            return this;
        }

        IATween IATween.OnKill(Action onKill)
        {
            return OnKill(onKill);
        }

        public Sequence OnStepComplete(Action onStepComplete)
        {
            this.onStepComplete = onStepComplete;
            return this;
        }

        IATween IATween.OnStepComplete(Action onStepComplete)
        {
            return OnStepComplete(onStepComplete);
        }

        public Sequence SetIgnoreTimeScale(bool ignoreTimeScale)
        {
            this.ignoreTimeScale = ignoreTimeScale;
            return this;
        }

        IATween IATween.SetIgnoreTimeScale(bool ignoreTimeScale)
        {
            return SetIgnoreTimeScale(ignoreTimeScale);
        }

        public Sequence SetLoops(int loops)
        {
            this.loops = loops;
            return this;
        }

        IATween IATween.SetLoops(int loops)
        {
            return SetLoops(loops);
        }

        public Sequence Play()
        {
            ATween.Instance.Commit(TweenCoroutine());
            this.isPlaying = true;
            this.pause = false;
            this.kill = false;
            this.complete = false;
            return this;
        }

        public Sequence Pause()
        {
            this.pause = true;
            return this;
        }

        IATween IATween.Pause()
        {
            return Pause();
        }

        public void Kill()
        {
            this.kill = true;
        }

        public void Complete()
        {
            this.complete = true;
        }

        public void Restart()
        {
            this.pause = false;
        }

        public void TogglePause()
        {
            this.pause = !this.pause;
        }

        IATween IATween.Play()
        {
            Play();
            return this;
        }

        private Sequence Append(ISequenceable sequenceable)
        {
            queue.AddLast(sequenceable);
            return this;
        }

        public Sequence Append(IATween tween)
        {
            return Append(new SequenceableTween(tween));
        }

        public Sequence AppendCallback(Action callback)
        {
            return Append(new SequenceableAction(callback));
        }

        public Sequence AppendInterval(float interval)
        {
            return Append(new SequenceableInterval(interval, this));
        }

        public Sequence Join(IATween tween)
        {
            return Append(new JoinedSequenceable(new SequenceableTween(tween)));
        }

        public IEnumerator TweenCoroutine()
        {
            if (onStart != null) onStart();

            yield return TweenCoroutineCore();

            while (loops != 0)
            {
                yield return TweenCoroutineCore();
            }

            if (onComplete != null) onComplete();

            isPlaying = false;
        }

        private IEnumerable TweenCoroutineCore()
        {
            while (queue.Count != 0)
            {
                var itemList = new List<ISequenceable>();
                itemList.Add(queue.First.Value);
                queue.RemoveFirst();

                while (queue.Count != 0 && queue.First.Value.IsJoined())
                {
                    itemList.Add(queue.First.Value);
                    queue.RemoveFirst();
                }

                foreach (var item in itemList)
                {
                    item.Play();
                }

                while (itemList.Any(i => i.IsPlaying()))
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

                    if (onUpdate != null) onUpdate();
                    yield return null;
                }
            }

            if (onStepComplete != null) onStepComplete();

            if (loops > 0)
            {
                --loops;
            }
        }

        List<ISequenceable> playingList = new List<ISequenceable>();

        public void Flip()
        {
            this.isBackwise = !this.isBackwise;
            foreach (var i in playingList)
            {
            }
        }

        private class SequenceableTween : ISequenceable
        {
            private readonly IATween tween;

            public SequenceableTween(IATween tween)
            {
                this.tween = tween;
            }

            public bool IsJoined() { return false; }

            public bool IsPlaying() { return tween.IsPlaying(); }

            public void Play() { tween.Play(); }
        }

        private class SequenceableAction : ISequenceable
        {
            private readonly Action action;

            public SequenceableAction(Action action)
            {
                this.action = action;
            }

            public bool IsJoined() { return false; }

            public bool IsPlaying() { return false; }

            public void Play() { action(); }
        }

        private class SequenceableInterval : ISequenceable
        {
            private readonly float interval;
            private readonly Sequence parent;
            private float elapsedTime;

            public SequenceableInterval(float interval, Sequence parent)
            {
                this.interval = interval;
                this.parent = parent;
            }

            public bool IsJoined() { return false; }

            public bool IsPlaying()
            {
                elapsedTime += parent.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                return elapsedTime < interval;
            }

            public void Play() { elapsedTime = 0f; }
        }

        private class JoinedSequenceable : ISequenceable
        {
            private readonly ISequenceable source;

            public JoinedSequenceable(ISequenceable source)
            {
                this.source = source;
            }

            public bool IsJoined() { return true; }

            public bool IsPlaying() { return source.IsPlaying(); }

            public void Play() { source.Play(); }
        }
    }

    public static class Ease
    {
        public static readonly IEase Linear = new Linear();
        public static readonly IEase InQuad = new InQuad();
        public static readonly IEase OutQuad = new OutQuad();
        public static readonly IEase InOutQuad = new InOutQuad();
        public static readonly IEase InCubic = new InCubic();
        public static readonly IEase OutCubic = new OutCubic();
        public static readonly IEase InOutCubic = new InOutCubic();
        public static readonly IEase InQuart = new InQuart();
        public static readonly IEase OutQuart = new OutQuart();
        public static readonly IEase InOutQuart = new InOutQuart();
        public static readonly IEase InQuint = new InQuint();
        public static readonly IEase OutQuint = new OutQuint();
        public static readonly IEase InOutQuint = new InOutQuint();
        public static readonly IEase InSine = new InSine();
        public static readonly IEase OutSine = new OutSine();
        public static readonly IEase InOutSine = new InOutSine();
        public static readonly IEase InExpo = new InExpo();
        public static readonly IEase OutExpo = new OutExpo();
        public static readonly IEase InOutExpo = new InOutExpo();
        public static readonly IEase InCirc = new InCirc();
        public static readonly IEase OutCirc = new OutCirc();
        public static readonly IEase InOutCirc = new InOutCirc();
    }

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

    namespace Core
    {
        public interface IEase
        {
            float Invoke(float t);
        }

        public class AnimationCurveEase : IEase
        {
            readonly AnimationCurve animationCurve;

            public AnimationCurveEase(AnimationCurve animationCurve)
            {
                this.animationCurve = animationCurve;
            }

            public float Invoke(float t)
            {
                return animationCurve.Evaluate(t);
            }
        }

        public class Linear : IEase
        {
            public float Invoke(float t)
            {
                return t;
            }
        }

        public class InQuad : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t, 2f);
            }
        }

        public class OutQuad : IEase
        {
            public float Invoke(float t)
            {
                return -(t * (t - 2f));
            }
        }

        public class InOutQuad : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return Mathf.Pow(t, 2f) * 2f;
                }
                else
                {
                    return (-2f * t * t) + (4f * t) - 1f;
                }
            }
        }

        public class InCubic : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t, 3f);
            }
        }

        public class OutCubic : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t - 1f, 3f) + 1f;
            }
        }

        public class InOutCubic : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return Mathf.Pow(t, 3f) * 4f;
                }
                else
                {
                    return Mathf.Pow((t * 2f) - 2f, 3f) * 0.5f + 1f;
                }
            }
        }

        public class InQuart : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t, 4f);
            }
        }

        public class OutQuart : IEase
        {
            public float Invoke(float t)
            {
                return 1f - Mathf.Pow(t - 1f, 4);
            }
        }

        public class InOutQuart : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return Mathf.Pow(t, 4f) * 8f;
                }
                else
                {
                    return Mathf.Pow(t - 1f, 4f) * -8f + 1f;
                }
            }
        }

        public class InQuint : IEase
        {
            public float Invoke(float t)
            {
                return t * t * t * t * t;
            }
        }

        public class OutQuint : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t - 1f, 5f) + 1f;
            }
        }

        public class InOutQuint : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return Mathf.Pow(t, 5f) * 16f;
                }
                else
                {
                    return Mathf.Pow((t * 2f) - 2f, 5f) * 0.5f + 1f;
                }
            }
        }

        public class InSine : IEase
        {
            public float Invoke(float t)
            {
                return -Mathf.Cos((t) * Mathf.PI * 0.5f) + 1f;
            }
        }

        public class OutSine : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Sin(t * Mathf.PI * 0.5f);
            }
        }

        public class InOutSine : IEase
        {
            public float Invoke(float t)
            {
                return 0.5f * (1f - Mathf.Cos(t * Mathf.PI));
            }
        }

        public class InExpo : IEase
        {
            public float Invoke(float t)
            {
                return t == 0f ? 0f : Mathf.Pow(2f, 10f * (t - 1f));
            }
        }

        public class OutExpo : IEase
        {
            public float Invoke(float t)
            {
                return t == 1f ? 1f : (-Mathf.Pow(2f, -10f * t) + 1f);
            }
        }

        public class InOutExpo : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return t == 0f ? 0f : (0.5f * Mathf.Pow(2f, (20f * t) - 10f));
                }
                else
                {
                    return t == 1f ? 1f : (-0.5f * Mathf.Pow(2f, (-20f * t) + 10f) + 1f);
                }
            }
        }

        public class InCirc : IEase
        {
            public float Invoke(float t)
            {
                return 1f - Mathf.Sqrt(1f - (t * t));
            }
        }

        public class OutCirc : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Sqrt((2f - t) * t);
            }
        }

        public class InOutCirc : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return 0.5f * (1f - Mathf.Sqrt(1f - 4f * (t * t)));
                }
                else
                {
                    return 0.5f * (Mathf.Sqrt(-((2f * t) - 3f) * ((2f * t) - 1f)) + 1f);
                }
            }
        }

        public interface ISequenceable
        {
            bool IsJoined();

            bool IsPlaying();

            void Play();
        }

        public static class TweenPlugin
        {
            public static readonly ITweenPlugin<float, float> Float = new FloatPlugin();
            public static readonly ITweenPlugin<double, double> Double = new DoublePlugin();
            public static readonly ITweenPlugin<int, int> Int = new IntPlugin();
            public static readonly ITweenPlugin<uint, uint> Uint = new UintPlugin();
            public static readonly ITweenPlugin<long, long> Long = new LongPlugin();
            public static readonly ITweenPlugin<ulong, ulong> Ulong = new UlongPlugin();
            public static readonly ITweenPlugin<Vector2, Vector2> Vector2 = new Vector2Plugin();
            public static readonly ITweenPlugin<Vector3, Vector3> Vector3 = new Vector3Plugin();
            public static readonly ITweenPlugin<Vector4, Vector4> Vector4 = new Vector4Plugin();
            public static readonly ITweenPlugin<Color, Color> Color = new ColorPlugin();
            public static readonly ITweenPlugin<Vector3, Quaternion> Quaternion = new QuaternionPlugin();
        }

        public interface ITweenPlugin<T1, T2>
        {
            T1 Lerp(T1 begin, T1 end, float t);
            T1 RelativeValue(T1 begin, T1 end);
            T1 IncrementalValue(T1 begin, T1 end);
            T1 Convert(T2 value);
            T2 Convert(T1 value);
            bool Equals(T1 value1, T2 value2);
        }

        public struct FloatPlugin : ITweenPlugin<float, float>
        {
            public float Lerp(float begin, float end, float t)
            {
                return Mathf.Lerp(begin, end, t);
            }

            public float RelativeValue(float begin, float end)
            {
                return end - begin;
            }

            public float IncrementalValue(float begin, float end)
            {
                return end + RelativeValue(begin, end);
            }

            public float Convert(float value)
            {
                return value;
            }

            public bool Equals(float value1, float value2)
            {
                return value1.Equals(value2);
            }
        }

        public struct DoublePlugin : ITweenPlugin<double, double>
        {
            public double Lerp(double begin, double end, float t)
            {
                return Mathf.Lerp((float)begin, (float)end, t);
            }

            public double RelativeValue(double begin, double end)
            {
                return end - begin;
            }

            public double IncrementalValue(double begin, double end)
            {
                return end + RelativeValue(begin, end);
            }
            public double Convert(double value)
            {
                return value;
            }

            public bool Equals(double value1, double value2)
            {
                return value1.Equals(value2);
            }
        }

        public struct IntPlugin : ITweenPlugin<int, int>
        {
            public int Lerp(int begin, int end, float t)
            {
                return (int)Mathf.Lerp(begin, end, t);
            }

            public int RelativeValue(int begin, int end)
            {
                return end - begin;
            }

            public int IncrementalValue(int begin, int end)
            {
                return end + RelativeValue(begin, end);
            }

            public int Convert(int value)
            {
                return value;
            }

            public bool Equals(int value1, int value2)
            {
                return value1.Equals(value2);
            }
        }

        public struct UintPlugin : ITweenPlugin<uint, uint>
        {
            public uint Lerp(uint begin, uint end, float t)
            {
                return (uint)Mathf.Lerp(begin, end, t);
            }

            public uint RelativeValue(uint begin, uint end)
            {
                return end - begin;
            }

            public uint IncrementalValue(uint begin, uint end)
            {
                return end + RelativeValue(begin, end);
            }

            public uint Convert(uint value)
            {
                return value;
            }

            public bool Equals(uint value1, uint value2)
            {
                return value1.Equals(value2);
            }
        }

        public struct LongPlugin : ITweenPlugin<long, long>
        {
            public long Lerp(long begin, long end, float t)
            {
                return (long)Mathf.Lerp(begin, end, t);
            }

            public long RelativeValue(long begin, long end)
            {
                return end - begin;
            }

            public long IncrementalValue(long begin, long end)
            {
                return end + RelativeValue(begin, end);
            }

            public long Convert(long value)
            {
                return value;
            }

            public bool Equals(long value1, long value2)
            {
                return value1.Equals(value2);
            }
        }

        public struct UlongPlugin : ITweenPlugin<ulong, ulong>
        {
            public ulong Lerp(ulong begin, ulong end, float t)
            {
                return (ulong)Mathf.Lerp(begin, end, t);
            }

            public ulong RelativeValue(ulong begin, ulong end)
            {
                return end - begin;
            }

            public ulong IncrementalValue(ulong begin, ulong end)
            {
                return end + RelativeValue(begin, end);
            }

            public ulong Convert(ulong value)
            {
                return value;
            }

            public bool Equals(ulong value1, ulong value2)
            {
                return value1.Equals(value2);
            }
        }

        public struct Vector2Plugin : ITweenPlugin<Vector2, Vector2>
        {
            public Vector2 Lerp(Vector2 begin, Vector2 end, float t)
            {
                return Vector2.Lerp(begin, end, t);
            }

            public Vector2 RelativeValue(Vector2 begin, Vector2 end)
            {
                return end - begin;
            }

            public Vector2 IncrementalValue(Vector2 begin, Vector2 end)
            {
                return end + RelativeValue(begin, end);
            }

            public Vector2 Convert(Vector2 value)
            {
                return value;
            }

            public bool Equals(Vector2 value1, Vector2 value2)
            {
                return value1 == value2;
            }
        }

        public struct Vector3Plugin : ITweenPlugin<Vector3, Vector3>
        {
            public Vector3 Lerp(Vector3 begin, Vector3 end, float t)
            {
                return Vector3.Lerp(begin, end, t);
            }

            public Vector3 RelativeValue(Vector3 begin, Vector3 end)
            {
                return end - begin;
            }

            public Vector3 IncrementalValue(Vector3 begin, Vector3 end)
            {
                return end + RelativeValue(begin, end);
            }

            public Vector3 Convert(Vector3 value)
            {
                return value;
            }

            public bool Equals(Vector3 value1, Vector3 value2)
            {
                return value1 == value2;
            }
        }

        public struct Vector4Plugin : ITweenPlugin<Vector4, Vector4>
        {
            public Vector4 Lerp(Vector4 begin, Vector4 end, float t)
            {
                return Vector4.Lerp(begin, end, t);
            }

            public Vector4 RelativeValue(Vector4 begin, Vector4 end)
            {
                return end - begin;
            }

            public Vector4 IncrementalValue(Vector4 begin, Vector4 end)
            {
                return end + RelativeValue(begin, end);
            }

            public Vector4 Convert(Vector4 value)
            {
                return value;
            }

            public bool Equals(Vector4 value1, Vector4 value2)
            {
                return value1 == value2;
            }
        }

        public struct ColorPlugin : ITweenPlugin<Color, Color>
        {
            public Color Lerp(Color begin, Color end, float t)
            {
                return Color.Lerp(begin, end, t);
            }

            public Color RelativeValue(Color begin, Color end)
            {
                return end - begin;
            }

            public Color IncrementalValue(Color begin, Color end)
            {
                return end + RelativeValue(begin, end);
            }

            public Color Convert(Color value)
            {
                return value;
            }

            public bool Equals(Color value1, Color value2)
            {
                return value1 == value2;
            }
        }

        public struct QuaternionPlugin : ITweenPlugin<Vector3, Quaternion>
        {
            public Vector3 Lerp(Vector3 begin, Vector3 end, float t)
            {
                return Vector3.Lerp(begin, end, t);
            }

            public Vector3 RelativeValue(Vector3 begin, Vector3 end)
            {
                return begin + end;
            }

            public Vector3 IncrementalValue(Vector3 begin, Vector3 end)
            {
                return end + RelativeValue(begin, end);
            }

            public Quaternion Convert(Vector3 value)
            {
                return Quaternion.Euler(value);
            }

            public Vector3 Convert(Quaternion value)
            {
                return value.eulerAngles;
            }

            public bool Equals(Vector3 value1, Quaternion value2)
            {
                return value1 == value2.eulerAngles;
            }
        }
    }
}