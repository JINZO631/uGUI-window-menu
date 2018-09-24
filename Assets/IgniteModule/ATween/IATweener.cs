using System;
using System.Collections;
using ATweening.Core;
using UnityEngine;

namespace ATweening
{
    public interface IATweener : IATween
    {
        IATweener SetEase(IEase ease);
        IATweener SetEase(AnimationCurve ease);
    }
}