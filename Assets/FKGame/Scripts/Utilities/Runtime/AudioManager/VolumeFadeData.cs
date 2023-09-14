namespace FKGame
{
    public class VolumeFadeData
    {
        public AudioAsset au;
        public float fadeTime;
        public float tempVolume;                                // 记录临时音量
        public float delayTime;                                 // 延迟播放
        public VolumeFadeType fadeType;
        public VolumeFadeStateType fadeState;
        public CallBack<AudioAsset> fadeCompleteCallBack;
        public CallBack<AudioAsset> fadeOutCompleteCallBack;    // 用于VolumeFadeType.FadeOut2In 当fade out完成时回调
    }
}