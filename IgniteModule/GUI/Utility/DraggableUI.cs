using UnityEngine.EventSystems;
using UnityEngine;

namespace IgniteModule.GUICore
{
    public class DraggableUI : GUIMonoBehaviour, IDragHandler, IBeginDragHandler
    {
        Vector2 offset;

        public void OnBeginDrag(PointerEventData eventData)
        {
            offset = RectTransform.position - (Vector3)eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform.position = eventData.position + offset;
        }
    }
}