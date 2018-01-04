using UnityEngine.EventSystems;
using UnityEngine;

namespace IgniteModule.UI
{
    public class Draggable : UIMonoBehaviour, IDragHandler, IBeginDragHandler
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