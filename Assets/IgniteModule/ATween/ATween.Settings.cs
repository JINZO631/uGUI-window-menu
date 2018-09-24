using UnityEngine;
using ATweening.Core;

namespace ATweening
{
    /*
        ATweenの全体設定
     */
    public partial class ATween : MonoBehaviour
    {
        /// <summary>
        /// デフォルトのイージング設定
        /// </summary>
        public static IEase DefaultEase = Ease.OutQuint;

        /// <summary>
        /// すべてのATweenでタイムスケールを無視するか
        /// </summary>
        public static bool IgnoreTimeScale = false;

        /// <summary>
        /// デフォルトの自動Tween再生設定
        /// </summary>
        public static bool DefaultAutoPlay = true;
    }
}