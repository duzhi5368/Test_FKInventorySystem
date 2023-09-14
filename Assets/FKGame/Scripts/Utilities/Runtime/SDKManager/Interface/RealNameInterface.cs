namespace FKGame
{
    // ʵ����״̬
    public enum RealNameStatus
    {
        IsRealName,     // �Ѿ�ʵ����
        NotRealName,    // δʵ����
        NotNeed,        // ����Ҫʵ����
    }

    [System.Serializable]
    public class RealNameInterface : SDKInterfaceBase
    {
        public override void Init()
        {
            base.Init();
        }

        // ��¼
        public virtual void OnLogin(string userID){}

        // �ǳ�
        public virtual void OnLogout(){}

        // ʵ����״̬
        public virtual RealNameStatus GetRealNameType(){ return RealNameStatus.NotNeed; }

        // �Ƿ����
        public virtual bool IsAdult(){ return true; }

        // ��������ʱ��
        public virtual int GetTodayOnlineTime(){ return -1; }

        // ��ʼʵ����
        public virtual void StartRealNameAttestation(){}

        // ���֧���Ƿ�����
        public virtual void CheckPayLimit(int payAmount)
        {
            if (SDKManager.PayLimitCallBack != null)
            {
                SDKManager.PayLimitCallBack(false, payAmount);
            }
        }

        // �ϱ�֧�����
        public virtual void LogPayAmount(int payAmount){}

        // �����ǳ�
        public virtual void RealNameLogoutCallBack()
        {
            if (SDKManager.RealNameLogoutCallBack != null)
            {
                SDKManager.RealNameLogoutCallBack();
            }
        }
    }
}