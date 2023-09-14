namespace FKGame
{
    // 实名制状态
    public enum RealNameStatus
    {
        IsRealName,     // 已经实名制
        NotRealName,    // 未实名制
        NotNeed,        // 不需要实名制
    }

    [System.Serializable]
    public class RealNameInterface : SDKInterfaceBase
    {
        public override void Init()
        {
            base.Init();
        }

        // 登录
        public virtual void OnLogin(string userID){}

        // 登出
        public virtual void OnLogout(){}

        // 实名制状态
        public virtual RealNameStatus GetRealNameType(){ return RealNameStatus.NotNeed; }

        // 是否成年
        public virtual bool IsAdult(){ return true; }

        // 今日在线时长
        public virtual int GetTodayOnlineTime(){ return -1; }

        // 开始实名制
        public virtual void StartRealNameAttestation(){}

        // 检测支付是否受限
        public virtual void CheckPayLimit(int payAmount)
        {
            if (SDKManager.PayLimitCallBack != null)
            {
                SDKManager.PayLimitCallBack(false, payAmount);
            }
        }

        // 上报支付金额
        public virtual void LogPayAmount(int payAmount){}

        // 触发登出
        public virtual void RealNameLogoutCallBack()
        {
            if (SDKManager.RealNameLogoutCallBack != null)
            {
                SDKManager.RealNameLogoutCallBack();
            }
        }
    }
}