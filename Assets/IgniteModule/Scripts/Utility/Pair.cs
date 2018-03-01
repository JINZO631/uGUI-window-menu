namespace IgniteModule
{
    public struct Pair<T1, T2>
    {
        public T1 first;
        public T2 second;
    }

    public static class Pair
    {
        public static Pair<T1, T2> Create<T1, T2>(T1 first, T2 second)
        {
            return new Pair<T1, T2>() { first = first, second = second };
        }
    }
}