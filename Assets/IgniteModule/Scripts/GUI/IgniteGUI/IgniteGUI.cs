using IgniteModule.UI.Detail;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public static partial class IgniteGUI
    {
        static Canvas canvas;
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
                var go = new GameObject("IgniteGUICanvas");
                canvas = go.AddComponent<Canvas>().InitCanvas();
                go.AddComponent<CanvasScaler>().InitCanvasScaler();
                go.AddComponent<GraphicRaycaster>().InitGraphicRaycaster();
                GameObject.DontDestroyOnLoad(go);
                return canvas;
            }
        }
        
        public static EventSystem GetOrAddEventSystem()
        {
            var eventSystem = GameObject.FindObjectOfType<EventSystem>();
            if (eventSystem != null)
            {
                return eventSystem;
            }
            var go = new GameObject("EventSystem");
            eventSystem = go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
            go.AddComponent<BaseInput>();
            return eventSystem;
        }


        static Dictionary<int, IgniteWindow> activeWindow;
        public static Dictionary<int, IgniteWindow> ActiveWindow { get { return activeWindow ?? (activeWindow = new Dictionary<int, IgniteWindow>());}}

        static Vector2 nextWindowPos = Vector2.zero;
        public static void SetWindowPos(IgniteWindow window)
        {
            window.RectTransform.anchoredPosition = nextWindowPos;
            nextWindowPos += new Vector2(Size.ElementHeight, -Size.ElementHeight);
        }


        public static void AddWindow(int key, IgniteWindow window)
        {
            window.Transform.SetParent(Canvas.transform);
            ActiveWindow.Add(key, window);
            window.Select.Where(v => v).Subscribe(_ =>
            {
                foreach(var i in ActiveWindow.Where(kvp => kvp.Key != key))
                {
                    i.Value.Select.Value = false;
                }
            })
            .AddTo(window);
        }

        static IIgniteGUISize size;
        public static IIgniteGUISize Size
        {
            get { return size != null ? size : (size = IgniteGUISize.Default); }
            set { size = value; }
        }

        static IIgniteGUITheme theme;
        public static IIgniteGUITheme Theme
        {
            get { return theme != null ? theme : (theme = IgniteGUITheme.Default); }
            set { theme = value; }
        }
    }

    namespace Detail
    {
        // Init Canvas Component
        public static partial class IgniteGUIComponentInitializer
        {
            public static Canvas InitCanvas(this Canvas canvas)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.pixelPerfect = false;
                canvas.sortingOrder = 100;
                return canvas;
            }

            public static CanvasScaler InitCanvasScaler(this CanvasScaler scaler)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                scaler.scaleFactor = 1;
                scaler.referencePixelsPerUnit = 100;
                return scaler;
            }

            public static GraphicRaycaster InitGraphicRaycaster(this GraphicRaycaster raycaster)
            {
                raycaster.ignoreReversedGraphics = true;
                raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
                return raycaster;
            }
        }
    }
}