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
        [SerializeField] Vector2 maxSize = new Vector2(1920, 1080);

        public SizeChangeEvent OnSizeChange { get; } = new SizeChangeEvent();

        void Start()
        {
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
