using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace IgniteModule.GUICore
{
    public class VariableSizePanel : GUIMonoBehaviour
    {
        [SerializeField] DragEventComponent dragEvent = null;
        [SerializeField] Vector2 minSize = new Vector2(100, 100);
        public SizeChangeEvent OnSizeChange { get; } = new SizeChangeEvent();
        Vector2 maxSize;

        void Start()
        {
            maxSize = new Vector2(Screen.width, Screen.height);
            dragEvent.OnDragEvent.AddListener(eventData =>
            {
                var sizeDelta = eventData.delta;
                sizeDelta.y = eventData.delta.y * -1;
                RectTransform.sizeDelta += sizeDelta;
                RectTransform.SetSizeDelta(Mathf.Clamp(RectTransform.sizeDelta.x, minSize.x, maxSize.x), Mathf.Clamp(RectTransform.sizeDelta.y, minSize.y, maxSize.y));
                OnSizeChange.Invoke(RectTransform.sizeDelta);
            });
        }

        public class SizeChangeEvent : UnityEvent<Vector2> { }
    }
}
