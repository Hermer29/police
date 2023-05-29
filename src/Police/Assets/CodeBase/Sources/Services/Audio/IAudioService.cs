namespace Logic.Audio
{
    public interface IAudioService
    {
        void ChangeState(bool state);
        bool IsEnabled { get; }
    }
}