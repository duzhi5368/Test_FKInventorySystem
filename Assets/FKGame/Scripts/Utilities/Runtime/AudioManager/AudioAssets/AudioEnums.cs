namespace FKGame
{
    // 音乐播放状态
    public enum AudioPlayState
    {
        Playing,
        Pause,
        Stoping,
        Stop,
    }

    // 音乐资源类型，音乐还是音效
    public enum AudioSourceType
    {
        Music,
        SFX,
    }

    public enum VolumeFadeType
    {
        FadeIn,
        FadeOut,
        FadeOut2In,
    }
    public enum VolumeFadeStateType
    {
        FadeIn,
        FadeOut,
        Delay,
        Complete,
    }
}