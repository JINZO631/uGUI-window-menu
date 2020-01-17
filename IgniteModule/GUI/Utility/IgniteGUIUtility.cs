using System;
using System.Collections;

namespace IgniteModule
{
    public static partial class IgniteGUIUtility
    {
        public static IEnumerator DelayedAction(Action action)
        {
            yield return null;
            action();
        }
    }
}