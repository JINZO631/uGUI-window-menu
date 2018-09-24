using UnityEngine;

namespace IgniteModule
{
    public interface IIgniteGUIGroup
    {
        IIgniteGUIGroup Parent { get; }
        IIgniteGUIGroup LastNestedGroup { get; }
        RectTransform Content { get; }
        IgniteWindow Window { get; }
        IIgniteGUIGroup Add(IIgniteGUIElement element);
    }
}