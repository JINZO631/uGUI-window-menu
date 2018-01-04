using UnityEngine;
using UnityEngine.EventSystems;

namespace IgniteModule.UI
{
    public class UIMonoBehaviour : UIBehaviour
    {
        private RectTransform _rectTransform = null;

        public RectTransform RectTransform
        {
            get
            {
                return _rectTransform == null ? (_rectTransform = GetComponent<RectTransform>()) : _rectTransform;
            }
        }

        private Transform _transform = null;

        public Transform Transform
        {
            get
            {
                return _transform == null ? (_transform = GetComponent<Transform>()) : _transform;
            }
        }
    }
}

