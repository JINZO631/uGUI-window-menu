namespace IgniteModule.UI
{
    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup WindowContentFit(this IIgniteGUIGroup group)
        {
            return group.Window.ContentFit();
        }

        public static IIgniteGUIGroup WindowFit(this IIgniteGUIGroup group)
        {
            return group.Window.Fit();
        }
    }
}