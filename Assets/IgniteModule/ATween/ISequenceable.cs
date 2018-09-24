namespace ATweening.Core
{
    public interface ISequenceable
    {
        bool IsJoined();

        bool IsPlaying();

        void Play();
    }
}