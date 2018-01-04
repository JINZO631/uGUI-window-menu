using UnityEngine;
using System.Linq;

namespace IgniteModule
{
    public static partial class TransformExtensions
    {
        #region Reset

        public static void ResetLoacal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale    = Vector3.one;
        }

        public static void ResetWorld(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        #endregion

        public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.position;
            if (x.HasValue) v.x = x.Value;
            if (y.HasValue) v.y = y.Value;
            if (z.HasValue) v.z = z.Value;
            transform.position = v;
        }


        public static void AddPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.position;
            if (x.HasValue) v.x += x.Value;
            if (y.HasValue) v.y += y.Value;
            if (z.HasValue) v.z += z.Value;
            transform.position = v;
        }

        public static void SetLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.localPosition;
            if (x.HasValue) v.x = x.Value;
            if (y.HasValue) v.y = y.Value;
            if (z.HasValue) v.z = z.Value;
            transform.localPosition = v;
        }

        public static void AddLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.localPosition;
            if (x.HasValue) v.x += x.Value;
            if (y.HasValue) v.y += y.Value;
            if (z.HasValue) v.z += z.Value;
            transform.localPosition = v;
        }

        public static void SetLocalScale(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.localScale;
            if (x.HasValue) v.x = x.Value;
            if (y.HasValue) v.y = y.Value;
            if (z.HasValue) v.z = z.Value;
            transform.localScale = v;
        }

        public static void AddLocalScale(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.localScale;
            if (x.HasValue) v.x += x.Value;
            if (y.HasValue) v.y += y.Value;
            if (z.HasValue) v.z += z.Value;
            transform.localScale = v;
        }

        public static void SetEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.eulerAngles;
            if (x.HasValue) v.x = x.Value;
            if (y.HasValue) v.y = y.Value;
            if (z.HasValue) v.z = z.Value;
            transform.eulerAngles = v;
        }

        public static void AddEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.eulerAngles;
            if (x.HasValue) v.x += x.Value;
            if (y.HasValue) v.y += y.Value;
            if (z.HasValue) v.z += z.Value;
            transform.eulerAngles = v;
        }

        public static void SetLocalEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.localEulerAngles;
            if (x.HasValue) v.x = x.Value;
            if (y.HasValue) v.y = y.Value;
            if (z.HasValue) v.z = z.Value;
            transform.localEulerAngles = v;
        }

        public static void AddLocalEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
        {
            var v = transform.localEulerAngles;
            if (x.HasValue) v.x += x.Value;
            if (y.HasValue) v.y += y.Value;
            if (z.HasValue) v.z += z.Value;
            transform.localEulerAngles = v;
        }

        #region GetChildren

        public static Transform[] GetChildren(this Transform transform)
        {
            return transform.GetComponentsInChildren<Transform>()
                            .Where(t => t.parent == transform)
                            .ToArray();
        }

        public static Transform[] GetChildren(this Transform transform, bool includeInactive)
        {
            return transform.GetComponentsInChildren<Transform>(includeInactive)
                            .Where(t => t.parent == transform)
                            .ToArray();
        }

        #endregion

        #region GetAllChildren

        public static Transform[] GetAllChildren(this Transform transform)
        {
            return transform.GetComponentsInChildren<Transform>()
                            .Where(t => t != transform)
                            .ToArray();
        }


        public static Transform[] GetAllChildren(this Transform transform, bool includeInactive)
        {
            return transform.GetComponentsInChildren<Transform>(includeInactive)
                            .Where(t => t != transform)
                            .ToArray();
        }
        #endregion
    }
}
