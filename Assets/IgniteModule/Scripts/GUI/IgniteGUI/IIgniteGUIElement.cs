using UnityEngine.UI;
using UnityEngine;
using System;
using UniRx;

namespace IgniteModule.UI
{
    public interface IIgniteGUIElement
    {
        string ID { get; }

        IgniteWindow Window { get; }

        RectTransform RectTransform { get; }

        void SetSize(IIgniteGUISize size);

        void SetTheme(IIgniteGUITheme theme);

        IObservable<Unit> OnSelected();
    }

    public interface IIgniteGUIElementGroup : IIgniteGUIElement, IIgniteGUIGroup
    {
    }
}