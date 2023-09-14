namespace FKGame
{
    public enum CameraFadeType
    {
        FadeIn,
        FadeOut,
    }

    public class CameraFadeData
    {
        public CameraFadeType fadeType;
        public bool isForceAlpha = false;       // �Ƿ�Ӳ���л�͸��ֵ��1,��0���ٽ���
        public float delay;
        public float fadeTime;
        public CallBack completeCallBack;
        public float tempFadeCaculateTime;

        public CameraFadeData() { }

        public CameraFadeData(CameraFadeType fadeType, bool isForceAlpha, float delay, float fadeTime, CallBack completeCallBack)
        {
            SetCameraFadeData(fadeType, isForceAlpha, delay, fadeTime, completeCallBack);
        }

        public void SetCameraFadeData(CameraFadeType fadeType, bool isForceAlpha, float delay, float fadeTime, CallBack completeCallBack)
        {
            this.fadeType = fadeType;
            this.isForceAlpha = isForceAlpha;
            this.delay = delay;
            this.fadeTime = fadeTime;
            this.completeCallBack = completeCallBack;
            tempFadeCaculateTime = fadeTime;
        }
    }
}