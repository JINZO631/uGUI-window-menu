using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using IgniteModule.GUICore;

namespace IgniteModule
{
    public static class IgniteGUI
    {
        static Canvas canvas;
        public static Dictionary<int, IgniteWindow> ActiveWindow { get; } = new Dictionary<int, IgniteWindow>();

        public static Canvas Canvas
        {
            get
            {
                if (!ActiveWindow.Any())
                {
                    GetOrAddEventSystem();
                }
                if (canvas != null)
                {
                    return canvas;
                }

                Debug.Log("IgniteGUI Canvas instantiate");

                ATweening.ATween.Initialize();

                var go = new GameObject("IgniteGUICanvas");

                canvas = go.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.pixelPerfect = false;
                canvas.sortingOrder = 100;

                var scaler = go.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                scaler.scaleFactor = 1;
                scaler.referencePixelsPerUnit = 100;

                var raycaster = go.AddComponent<GraphicRaycaster>();
                raycaster.ignoreReversedGraphics = true;
                raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

                GameObject.DontDestroyOnLoad(go);

                return canvas;

                EventSystem GetOrAddEventSystem()
                {
                    var eventSystem = GameObject.FindObjectOfType<EventSystem>();
                    if (eventSystem != null)
                    {
                        return eventSystem;
                    }

                    Debug.Log("IgniteGUI EventSystem instantiate");

                    var eventSystemGo = new GameObject("EventSystem");
                    eventSystem = eventSystemGo.AddComponent<EventSystem>();
                    eventSystemGo.AddComponent<StandaloneInputModule>();
                    eventSystemGo.AddComponent<BaseInput>();
                    return eventSystem;
                }
            }
        }

        public static void AddWindow(int key, IgniteWindow window)
        {
            window.Transform.SetParent(IgniteGUI.Canvas.transform);
            ActiveWindow.Add(key, window);
            window.OnSelect.AddListener(() =>
            {
                foreach (var i in ActiveWindow.Where(kvp => kvp.Key != key))
                {
                    i.Value.IsSelected = false;
                }
            });
        }

        static Vector2 nextWindowPos = Vector2.zero;
        public static void SetWindowPos(IgniteWindow window)
        {
            window.RectTransform.anchoredPosition = nextWindowPos;
            nextWindowPos += new Vector2(IgniteGUISettings.ElementHeight, -IgniteGUISettings.ElementHeight);
        }
    }
}