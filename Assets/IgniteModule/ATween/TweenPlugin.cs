using UnityEngine;

namespace ATweening.Core
{
    public static class TweenPlugin
    {
        public static readonly ITweenPlugin<float, float> Float = new FloatPlugin();
        public static readonly ITweenPlugin<double, double> Double = new DoublePlugin();
        public static readonly ITweenPlugin<int, int> Int = new IntPlugin();
        public static readonly ITweenPlugin<uint, uint> Uint = new UintPlugin();
        public static readonly ITweenPlugin<long, long> Long = new LongPlugin();
        public static readonly ITweenPlugin<ulong, ulong> Ulong = new UlongPlugin();
        public static readonly ITweenPlugin<Vector2, Vector2> Vector2 = new Vector2Plugin();
        public static readonly ITweenPlugin<Vector3, Vector3> Vector3 = new Vector3Plugin();
        public static readonly ITweenPlugin<Vector4, Vector4> Vector4 = new Vector4Plugin();
        public static readonly ITweenPlugin<Color, Color> Color = new ColorPlugin();
        public static readonly ITweenPlugin<Vector3, Quaternion> Quaternion = new QuaternionPlugin();
    }

    public interface ITweenPlugin<T1, T2>
    {
        T1 Lerp(T1 begin, T1 end, float t);
        T1 RelativeValue(T1 begin, T1 end);
        T1 IncrementalValue(T1 begin, T1 end);

        T1 Convert(T2 value);
        T2 Convert(T1 value);
    }

    public struct FloatPlugin : ITweenPlugin<float, float>
    {
        public float Lerp(float begin, float end, float t)
        {
            return Mathf.Lerp(begin, end, t);
        }

        public float RelativeValue(float begin, float end)
        {
            return end - begin;
        }

        public float IncrementalValue(float begin, float end)
        {
            return end + RelativeValue(begin, end);
        }

        public float Convert(float value)
        {
            return value;
        }
    }

    public struct DoublePlugin : ITweenPlugin<double, double>
    {
        public double Lerp(double begin, double end, float t)
        {
            return Mathf.Lerp((float)begin, (float)end, t);
        }

        public double RelativeValue(double begin, double end)
        {
            return end - begin;
        }

        public double IncrementalValue(double begin, double end)
        {
            return end + RelativeValue(begin, end);
        }
        public double Convert(double value)
        {
            return value;
        }
    }

    public struct IntPlugin : ITweenPlugin<int, int>
    {
        public int Lerp(int begin, int end, float t)
        {
            return (int)Mathf.Lerp(begin, end, t);
        }

        public int RelativeValue(int begin, int end)
        {
            return end - begin;
        }

        public int IncrementalValue(int begin, int end)
        {
            return end + RelativeValue(begin, end);
        }

        public int Convert(int value)
        {
            return value;
        }
    }

    public struct UintPlugin : ITweenPlugin<uint, uint>
    {
        public uint Lerp(uint begin, uint end, float t)
        {
            return (uint)Mathf.Lerp(begin, end, t);
        }

        public uint RelativeValue(uint begin, uint end)
        {
            return end - begin;
        }

        public uint IncrementalValue(uint begin, uint end)
        {
            return end + RelativeValue(begin, end);
        }

        public uint Convert(uint value)
        {
            return value;
        }
    }

    public struct LongPlugin : ITweenPlugin<long, long>
    {
        public long Lerp(long begin, long end, float t)
        {
            return (long)Mathf.Lerp(begin, end, t);
        }

        public long RelativeValue(long begin, long end)
        {
            return end - begin;
        }

        public long IncrementalValue(long begin, long end)
        {
            return end + RelativeValue(begin, end);
        }

        public long Convert(long value)
        {
            return value;
        }
    }

    public struct UlongPlugin : ITweenPlugin<ulong, ulong>
    {
        public ulong Lerp(ulong begin, ulong end, float t)
        {
            return (ulong)Mathf.Lerp(begin, end, t);
        }

        public ulong RelativeValue(ulong begin, ulong end)
        {
            return end - begin;
        }

        public ulong IncrementalValue(ulong begin, ulong end)
        {
            return end + RelativeValue(begin, end);
        }

        public ulong Convert(ulong value)
        {
            return value;
        }
    }

    public struct Vector2Plugin : ITweenPlugin<Vector2, Vector2>
    {
        public Vector2 Lerp(Vector2 begin, Vector2 end, float t)
        {
            return Vector2.Lerp(begin, end, t);
        }

        public Vector2 RelativeValue(Vector2 begin, Vector2 end)
        {
            return end - begin;
        }

        public Vector2 IncrementalValue(Vector2 begin, Vector2 end)
        {
            return end + RelativeValue(begin, end);
        }

        public Vector2 Convert(Vector2 value)
        {
            return value;
        }
    }

    public struct Vector3Plugin : ITweenPlugin<Vector3, Vector3>
    {
        public Vector3 Lerp(Vector3 begin, Vector3 end, float t)
        {
            return Vector3.Lerp(begin, end, t);
        }

        public Vector3 RelativeValue(Vector3 begin, Vector3 end)
        {
            return end - begin;
        }

        public Vector3 IncrementalValue(Vector3 begin, Vector3 end)
        {
            return end + RelativeValue(begin, end);
        }

        public Vector3 Convert(Vector3 value)
        {
            return value;
        }
    }

    public struct Vector4Plugin : ITweenPlugin<Vector4, Vector4>
    {
        public Vector4 Lerp(Vector4 begin, Vector4 end, float t)
        {
            return Vector4.Lerp(begin, end, t);
        }

        public Vector4 RelativeValue(Vector4 begin, Vector4 end)
        {
            return end - begin;
        }

        public Vector4 IncrementalValue(Vector4 begin, Vector4 end)
        {
            return end + RelativeValue(begin, end);
        }

        public Vector4 Convert(Vector4 value)
        {
            return value;
        }
    }

    public struct ColorPlugin : ITweenPlugin<Color, Color>
    {
        public Color Lerp(Color begin, Color end, float t)
        {
            return Color.Lerp(begin, end, t);
        }

        public Color RelativeValue(Color begin, Color end)
        {
            return end - begin;
        }

        public Color IncrementalValue(Color begin, Color end)
        {
            return end + RelativeValue(begin, end);
        }

        public Color Convert(Color value)
        {
            return value;
        }
    }

    public struct QuaternionPlugin : ITweenPlugin<Vector3, Quaternion>
    {
        public Vector3 Lerp(Vector3 begin, Vector3 end, float t)
        {
            return Vector3.Lerp(begin, end, t);
        }

        public Vector3 RelativeValue(Vector3 begin, Vector3 end)
        {
            return begin + end;
        }

        public Vector3 IncrementalValue(Vector3 begin, Vector3 end)
        {
            return end + RelativeValue(begin, end);
        }

        public Quaternion Convert(Vector3 value)
        {
            return Quaternion.Euler(value);
        }

        public Vector3 Convert(Quaternion value)
        {
            return value.eulerAngles;
        }
    }
}