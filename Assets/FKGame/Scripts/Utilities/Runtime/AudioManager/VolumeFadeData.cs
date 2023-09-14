namespace FKGame
{
    public class VolumeFadeData
    {
        public AudioAsset au;
        public float fadeTime;
        public float tempVolume;                                // ��¼��ʱ����
        public float delayTime;                                 // �ӳٲ���
        public VolumeFadeType fadeType;
        public VolumeFadeStateType fadeState;
        public CallBack<AudioAsset> fadeCompleteCallBack;
        public CallBack<AudioAsset> fadeOutCompleteCallBack;    // ����VolumeFadeType.FadeOut2In ��fade out���ʱ�ص�
    }
}