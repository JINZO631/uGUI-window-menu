using UnityEngine;

namespace IgniteModule
{
    public static class IgniteMath
    {
        public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
        {
            var x = Mathf.Clamp(value.x, min.x, max.x);
            var y = Mathf.Clamp(value.y, min.y, max.y);
            return new Vector2(x, y);
        }
    }
}