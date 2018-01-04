namespace IgniteModule
{
    public static partial class Params
    {
        public static TParam[] To<TParam>(params TParam[] @params)
        {
            return @params;
        }
    }
}