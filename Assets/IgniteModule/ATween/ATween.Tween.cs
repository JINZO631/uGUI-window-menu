using System;
using System.Collections;
using UnityEngine;
using ATweening.Core;

namespace ATweening
{
    public partial class ATween : MonoBehaviour
    {
        public static ATweener<float, float> To(Func<float> getter, Action<float> setter, float endValue, float duration)
        {
            return TweenCore<float, float>(endValue, duration, getter, setter, TweenPlugin.Float);
        }

        public static ATweener<double, double> To(Func<double> getter, Action<double> setter, double endValue, float duration)
        {
            return TweenCore<double, double>(endValue, duration, getter, setter, TweenPlugin.Double);
        }

        public static ATweener<int, int> To(Func<int> getter, Action<int> setter, int endValue, float duration)
        {
            return TweenCore<int, int>(endValue, duration, getter, setter, TweenPlugin.Int);
        }

        public static ATweener<uint, uint> To(Func<uint> getter, Action<uint> setter, uint endValue, float duration)
        {
            return TweenCore<uint, uint>(endValue, duration, getter, setter, TweenPlugin.Uint);
        }

        public static ATweener<long, long> To(Func<long> getter, Action<long> setter, long endValue, float duration)
        {
            return TweenCore<long, long>(endValue, duration, getter, setter, TweenPlugin.Long);
        }

        public static ATweener<ulong, ulong> To(Func<ulong> getter, Action<ulong> setter, ulong endValue, float duration)
        {
            return TweenCore<ulong, ulong>(endValue, duration, getter, setter, TweenPlugin.Ulong);
        }

        public static ATweener<Vector2, Vector2> To(Func<Vector2> getter, Action<Vector2> setter, Vector2 endValue, float duration)
        {
            return TweenCore<Vector2, Vector2>(endValue, duration, getter, setter, TweenPlugin.Vector2);
        }

        public static ATweener<Vector3, Vector3> To(Func<Vector3> getter, Action<Vector3> setter, Vector3 endValue, float duration)
        {
            return TweenCore<Vector3, Vector3>(endValue, duration, getter, setter, TweenPlugin.Vector3);
        }

        public static ATweener<Vector4, Vector4> To(Func<Vector4> getter, Action<Vector4> setter, Vector4 endValue, float duration)
        {
            return TweenCore<Vector4, Vector4>(endValue, duration, getter, setter, TweenPlugin.Vector4);
        }

        public static ATweener<Color, Color> To(Func<Color> getter, Action<Color> setter, Color endValue, float duration)
        {
            return TweenCore<Color, Color>(endValue, duration, getter, setter, TweenPlugin.Color);
        }

        public static ATweener<Vector3, Quaternion> To(Func<Quaternion> getter, Action<Quaternion> setter, Vector3 endValue, float duration)
        {
            return TweenCore<Vector3, Quaternion>(endValue, duration, getter, setter, TweenPlugin.Quaternion);
        }

        private static ATweener<T1, T2> TweenCore<T1, T2>(T1 endValue, float duration, Func<T2> getter, Action<T2> setter, ITweenPlugin<T1, T2> plugin)
        {
            var tweener = new ATweener<T1, T2>(default(T1), endValue, duration, getter, setter, plugin);
            tweeners.Add(tweener);
            return tweener;
        }
    }
}
