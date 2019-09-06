using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using IgniteModule.GUICore;
using System;
using System.Collections;

namespace IgniteModule
{
    public static class IgniteGUI
    {
        static RectTransform panelTransform;
        public static Dictionary<int, IgniteWindow> ActiveWindow { get; } = new Dictionary<int, IgniteWindow>();

        public static RectTransform WindowRoot
        {
            get
            {
                if (!ActiveWindow.Any())
                {
                    GetOrAddEventSystem();
                }
                if (panelTransform != null)
                {
                    return panelTransform;
                }

                Debug.Log("IgniteGUI Canvas instantiate");

                ATweening.ATween.Initialize();

                var canvasGameObject = new GameObject("IgniteGUICanvas");

                // Canvas
                var canvas = canvasGameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.pixelPerfect = false;
                canvas.sortingOrder = 100;

                // CanvasScaler
                var scaler = canvasGameObject.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
                scaler.scaleFactor = 1;
                scaler.referencePixelsPerUnit = 100;

                // GraphicRaycaster
                var raycaster = canvasGameObject.AddComponent<GraphicRaycaster>();
                raycaster.ignoreReversedGraphics = true;
                raycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

                // WindowRootRectTransform
                var panelGameObject = new GameObject("IgntieGUICanvasPanel");
                panelTransform = panelGameObject.AddComponent<RectTransform>();
                panelTransform.SetParent(canvasGameObject.transform);
                panelTransform.SetAnchorPreset(AnchorPresets.StretchAll, true, true);

#if UNITY_IOS
                // iPhoneX対応
                var safeArea = Screen.safeArea;
                var anchorMin = new Vector2(safeArea.position.x / Screen.width, safeArea.position.y / Screen.height);
                var anchorMax = new Vector2((safeArea.position.x + safeArea.size.x) / Screen.width, (safeArea.position.y + safeArea.size.y) / Screen.height);
                panelTransform.anchorMin = anchorMin;
                panelTransform.anchorMax = anchorMax;
#endif

                GameObject.DontDestroyOnLoad(canvasGameObject);

                return panelTransform;
            }
        }

        static EventSystem GetOrAddEventSystem()
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

        public static void AddWindow(int key, IgniteWindow window)
        {
            window.Transform.SetParent(IgniteGUI.WindowRoot);
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