using UnityEngine.UI;
using UniRx;

namespace IgniteModule
{
    public static partial class UGUIExtensions
    {
        public static IObservable<int> OnValueChangedAsObservable(this Dropdown dropdown)
        {
            return dropdown.onValueChanged.AsObservable();
        }
    }
}