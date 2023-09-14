namespace FKGame
{
    public enum ADType
    {
        Banner,
        Reward,
        Interstitial,
        Video,
    }
    public class ADInterface : SDKInterfaceBase
    {
        public CallBack m_ADLoadFinish;

        public override void Init(){}
        public virtual void LoadAD(ADType adType, string tag = ""){}
        public virtual void PlayAD(ADType adType, string tag = ""){}
        public virtual void CloseAD(ADType adType, string tag = ""){}
        public virtual bool IsLoaded(ADType adType, string tag = ""){return true;}
    }
}