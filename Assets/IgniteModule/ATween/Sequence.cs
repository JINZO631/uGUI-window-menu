using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ATweening.Core;

namespace ATweening
{
    public partial class ATween : MonoBehaviour
    {
        public static Sequence Sequence()
        {
            var sequence = new Sequence();

            return sequence;
        }
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
}