using UnityEngine;
using IgniteModule.GUICore;

namespace IgniteModule
{
    public static partial class IgniteGUI
    {
        private static Vector2 nextWindowPos = Vector2.zero;
        private static int setWindowCount = 0;
        public static void SetWindowPos(IgniteWindow window)
        {
            window.RectTransform.anchoredPosition = nextWindowPos;
            nextWindowPos += new Vector2(IgniteGUISettings.ElementHeight, -IgniteGUISettings.ElementHeight);
            setWindowCount++;

            if (setWindowCount >= 10)
            {
                setWindowCount = 0;
                nextWindowPos = new Vector2(0, nextWindowPos.y - IgniteGUISettings.ElementHeight);
            }
        }

        public static void ResetNextWindowPos()
        {
            nextWindowPos = Vector2.zero;
        }
    }
}