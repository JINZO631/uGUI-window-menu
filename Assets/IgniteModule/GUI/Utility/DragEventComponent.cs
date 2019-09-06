using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace IgniteModule.GUICore
{
    public class DragEventComponent : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public DragEvent OnDragEvent { get; } = new DragEvent();
        public EndDragEvent OnEndDragEvent { get; } = new EndDragEvent();

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDragEvent.Invoke(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent.Invoke(eventData);
        }

        public class DragEvent : UnityEvent<PointerEventData> { }
        public class EndDragEvent : UnityEvent<PointerEventData> { }
    }
}