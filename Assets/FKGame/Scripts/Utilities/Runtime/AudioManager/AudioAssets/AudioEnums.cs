namespace FKGame
{
    // ���ֲ���״̬
    public enum AudioPlayState
    {
        Playing,
        Pause,
        Stoping,
        Stop,
    }

    // ������Դ���ͣ����ֻ�����Ч
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