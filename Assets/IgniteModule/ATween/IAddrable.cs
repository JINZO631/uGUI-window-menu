namespace ATweening
{
    public interface IAddable<T> : System.IEquatable<T>
    {
        T Add(T other);
    }
}