using UnityEngine;

namespace IgniteModule.UI
{
    public interface IIgniteGUIGroup
    {
        IIgniteGUIGroup Parent { get; }
        IIgniteGUIGroup LastNestedGroup { get; }

        IgniteWindow Window { get; }

        IIgniteGUIGroup Add(IIgniteGUIElement element);

        IIgniteGUIElement FindChild(string id);
        TElement FindChild<TElement>(string id) where TElement : Component, IIgniteGUIElement;
    }
}