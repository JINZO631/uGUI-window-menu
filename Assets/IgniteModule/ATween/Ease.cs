using UnityEngine;
using ATweening.Core;

namespace ATweening
{
    public static class Ease
    {
        public static readonly IEase Linear = new Linear();
        public static readonly IEase InQuad = new InQuad();
        public static readonly IEase OutQuad = new OutQuad();
        public static readonly IEase InOutQuad = new InOutQuad();
        public static readonly IEase InCubic = new InCubic();
        public static readonly IEase OutCubic = new OutCubic();
        public static readonly IEase InOutCubic = new InOutCubic();
        public static readonly IEase InQuart = new InQuart();
        public static readonly IEase OutQuart = new OutQuart();
        public static readonly IEase InOutQuart = new InOutQuart();
        public static readonly IEase InQuint = new InQuint();
        public static readonly IEase OutQuint = new OutQuint();
        public static readonly IEase InOutQuint = new InOutQuint();
        public static readonly IEase InSine = new InSine();
        public static readonly IEase OutSine = new OutSine();
        public static readonly IEase InOutSine = new InOutSine();
        public static readonly IEase InExpo = new InExpo();
        public static readonly IEase OutExpo = new OutExpo();
        public static readonly IEase InOutExpo = new InOutExpo();
        public static readonly IEase InCirc = new InCirc();
        public static readonly IEase OutCirc = new OutCirc();
        public static readonly IEase InOutCirc = new InOutCirc();
    }

    namespace Core
    {
        public class AnimationCurveEase : IEase
        {
            readonly AnimationCurve animationCurve;

            public AnimationCurveEase(AnimationCurve animationCurve)
            {
                this.animationCurve = animationCurve;
            }

            public float Invoke(float t)
            {
                return animationCurve.Evaluate(t);
            }
        }

        public class Linear : IEase
        {
            public float Invoke(float t)
            {
                return t;
            }
        }

        public class InQuad : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t, 2f);
            }
        }

        public class OutQuad : IEase
        {
            public float Invoke(float t)
            {
                return -(t * (t - 2f));
            }
        }

        public class InOutQuad : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return Mathf.Pow(t, 2f) * 2f;
                }
                else
                {
                    return (-2f * t * t) + (4f * t) - 1f;
                }
            }
        }

        public class InCubic : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t, 3f);
            }
        }

        public class OutCubic : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t - 1f, 3f) + 1f;
            }
        }

        public class InOutCubic : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return Mathf.Pow(t, 3f) * 4f;
                }
                else
                {
                    return Mathf.Pow((t * 2f) - 2f, 3f) * 0.5f + 1f;
                }
            }
        }

        public class InQuart : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t, 4f);
            }
        }

        public class OutQuart : IEase
        {
            public float Invoke(float t)
            {
                return 1f - Mathf.Pow(t - 1f, 4);
            }
        }

        public class InOutQuart : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return Mathf.Pow(t, 4f) * 8f;
                }
                else
                {
                    return Mathf.Pow(t - 1f, 4f) * -8f + 1f;
                }
            }
        }

        public class InQuint : IEase
        {
            public float Invoke(float t)
            {
                return t * t * t * t * t;
            }
        }

        public class OutQuint : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Pow(t - 1f, 5f) + 1f;
            }
        }

        public class InOutQuint : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return Mathf.Pow(t, 5f) * 16f;
                }
                else
                {
                    return Mathf.Pow((t * 2f) - 2f, 5f) * 0.5f + 1f;
                }
            }
        }

        public class InSine : IEase
        {
            public float Invoke(float t)
            {
                return -Mathf.Cos((t) * Mathf.PI * 0.5f) + 1f;
            }
        }

        public class OutSine : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Sin(t * Mathf.PI * 0.5f);
            }
        }

        public class InOutSine : IEase
        {
            public float Invoke(float t)
            {
                return 0.5f * (1f - Mathf.Cos(t * Mathf.PI));
            }
        }

        public class InExpo : IEase
        {
            public float Invoke(float t)
            {
                return t == 0f ? 0f : Mathf.Pow(2f, 10f * (t - 1f));
            }
        }

        public class OutExpo : IEase
        {
            public float Invoke(float t)
            {
                return t == 1f ? 1f : (-Mathf.Pow(2f, -10f * t) + 1f);
            }
        }

        public class InOutExpo : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return t == 0f ? 0f : (0.5f * Mathf.Pow(2f, (20f * t) - 10f));
                }
                else
                {
                    return t == 1f ? 1f : (-0.5f * Mathf.Pow(2f, (-20f * t) + 10f) + 1f);
                }
            }
        }

        public class InCirc : IEase
        {
            public float Invoke(float t)
            {
                return 1f - Mathf.Sqrt(1f - (t * t));
            }
        }

        public class OutCirc : IEase
        {
            public float Invoke(float t)
            {
                return Mathf.Sqrt((2f - t) * t);
            }
        }

        public class InOutCirc : IEase
        {
            public float Invoke(float t)
            {
                if (t < 0.5f)
                {
                    return 0.5f * (1f - Mathf.Sqrt(1f - 4f * (t * t)));
                }
                else
                {
                    return 0.5f * (Mathf.Sqrt(-((2f * t) - 3f) * ((2f * t) - 1f)) + 1f);
                }
            }
        }
    }
}