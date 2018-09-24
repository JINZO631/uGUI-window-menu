using System;
using System.Collections;
using ATweening.Core;

namespace ATweening
{
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
}