using UnityEngine;
using System;

namespace IgniteModule
{
    public interface IIgniteGUIElement
    {
        IgniteWindow Window { get; }
        RectTransform RectTransform { get; }

        void SetParent(IIgniteGUIGroup parent);
        void OnSelected(Action onSelected);
    }

    public interface IIgniteGUIElementGroup : IIgniteGUIElement, IIgniteGUIGroup
    {
    }
}