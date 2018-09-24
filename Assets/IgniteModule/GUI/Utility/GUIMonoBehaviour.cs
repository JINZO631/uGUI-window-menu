using UnityEngine;

namespace IgniteModule.GUICore
{
    public class GUIMonoBehaviour : MonoBehaviour
    {
        Transform cacheTransform;
        public Transform Transform => cacheTransform == null ? (cacheTransform = GetComponent<Transform>()) : cacheTransform;

        RectTransform cacheRectTransform;
        public RectTransform RectTransform => cacheRectTransform == null ? (cacheRectTransform = GetComponent<RectTransform>()) : cacheRectTransform;
    }
}
