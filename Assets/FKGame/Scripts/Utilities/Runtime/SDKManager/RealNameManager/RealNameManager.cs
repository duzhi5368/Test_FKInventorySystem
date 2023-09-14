using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ǰ�˼��1minѯ�ʷ����������ݹ��򴥷�ʵ����֤���߼���Ӧ���� RealNameLimitEvent �¼��Դ���ʵ���ƹ��򴥷�����߼�����Ҫ��Ϸ�߼��㴦��
    // ͨ������ CheckPayLimit ��������������������֤�� ����CheckPayLimitResultEvent ��Ϣ����ȡ��� �������������Ĺ����߼���ϣ���Ϸ�߼��㲻��Ҫ����
    public class RealNameManager
    {
        static private RealNameManager instance;
        static public RealNameManager GetInstance()
        {
            if (instance == null)
            {
                instance = new RealNameManager();
            }
            return instance;
        }

        public bool isAdult = false;                                    // �Ƿ��ǳ�����
        private bool openRealName = false;                              // �Ƿ���ʵ����֤ ���ܿ���
        private RealNameStatus realNameStatus = RealNameStatus.NotNeed; // ʵ��״̬
        float c_onlineTimer = 1 * 60;                                   // ������Ϸʱ������    1���� * 60��/����  
        float onlineTimer = 0;

        public RealNameStatus RealNameStatus
        {
            get
            {
                return realNameStatus;
            }
            set
            {
                Debug.Log("Set RealNameStatus:" + value);
                realNameStatus = value;
            }
        }

        // �Ƿ���Ҫʵ����֤
        public bool IsOpenRealName()
        {
            return openRealName;
        }

        // �Ƿ��ǳ�����
        public bool IsAdult()
        {
            return isAdult;
        }

        // ��ʼʵ����֤
        public void StartRealNameAttestation()
        {
            if (openRealName && RealNameStatus == RealNameStatus.NotRealName)
            {
                SDKManager.StartRealNameAttestation();  // ����sdk ��ʼʵ����֤ 
                TestRealNameStatus();                   // ����֤����ϱ�������
            }
            else
            {
                Debug.LogWarning("StartRealNameVerify useless: openRealName is" + openRealName + " realNameStatus is " + RealNameStatus);
            }
        }

        // ����Ƿ���֧������  ��Ҫ���� CheckPayLimitResultEvent ��Ϣ����ȡ���
        public void CheckPayLimit(int payAmount)
        {
            CheckPayLimitEvent.Dispatch(payAmount);
        }

        public void Init()
        {
            // ����ܿ���
            TestRealNameSwitch();
            if (openRealName)
            {
                ApplicationManager.s_OnApplicationUpdate += OnUpdate;
                SDKManager.RealNameCallBack += OnRealNameCallBack;
                SDKManager.PayLimitCallBack += OnPayLimitCallBack;
                LoginGameController.OnUserLogin += OnLoginEvent;
                LoginGameController.OnUserLogout += OnLogoutEvent;
                SDKManager.RealNameLogoutCallBack += OnNeedLogout;
                AddNetEvent();
                // ���ʵ��״̬����������ʵ����
                InitOnlineTimer();
            }
            GlobalEvent.AddTypeEvent<CheckPayLimitEvent>(OnCheckPayLimit);
        }

        // ��Ҫ�ǳ��˺�
        private void OnNeedLogout()
        {
            Debug.Log("========RealNameManager OnNeedLogout");
            LoginGameController.Logout();
        }

        private void OnLogoutEvent(UserLogout2Client t)
        {
            SDKManager.RealNameLogout();
            RealNameStatus = RealNameStatus.NotNeed;    // �Ѿ��ǳ���ֹͣ���
        }

        private void OnLoginEvent(UserLogin2Client t)
        {
            Debug.Log("RealNameManager OnLoginEvent" + t.code + t.reloginState);
            if (t.code != 0 || t.reloginState)
                return;
            SDKManager.RealNameLogin(t.user.userID);
            TestRealNameStatus();
        }

        // ��ѯ֧������Ļص�
        private void OnPayLimitCallBack(bool isLimit, int payAmount)
        {
            PayLimitType payLimitType = PayLimitType.None;
            if (isLimit)
            {
                payLimitType = PayLimitType.ChildLimit;
            }
            Debug.Log("OnPayLimitCallBack from SDK:" + payLimitType);
            CheckPayLimitResultEvent.Dispatch(payAmount, payLimitType);
        }

        // SDK ʵ������֤�ص�
        private void OnRealNameCallBack(RealNameData info)
        {
            Debug.Log("OnRealNameCallBack" + info.realNameStatus + " isAdult:" + info.isAdult);
            RealNameStatus = info.realNameStatus;
            isAdult = info.isAdult;
        }

        // ���յ�ѯ��֧�����Ƶ��¼�
        private void OnCheckPayLimit(CheckPayLimitEvent e, object[] args)
        {
            PayLimitType payLimitType = PayLimitType.None;// Ĭ�ϲ���Ҫʵ����֤��������
            Debug.LogWarning("OnCheckPayLimit====openRealName==" + openRealName);
            if (openRealName)
            {
                if (RealNameStatus == RealNameStatus.NotRealName)
                {
                    StartRealNameAttestation(); // �Զ���ʼʵ������֤
                    payLimitType = PayLimitType.NoRealName;
                }
                else if (RealNameStatus == RealNameStatus.IsRealName)
                {
                    if (isAdult) // ����
                    {
                        payLimitType = PayLimitType.None;
                    }
                    else // δ����
                    {
                        CheckPayLimitBySDK(e.payAmount);
                        Debug.LogWarning("CheckPayLimitBySDK");
                        return;
                    }
                }
                else if (RealNameStatus == RealNameStatus.NotNeed)
                {
                    payLimitType = PayLimitType.None;// Ĭ�ϲ���Ҫʵ����֤��������
                }
            }
            Debug.LogWarning("OnCheckPayLimit====payLimitType==" + payLimitType);
            CheckPayLimitResultEvent.Dispatch(e.payAmount, payLimitType);
        }

        private void OnUpdate()
        {
            OnlineTimer();
        }

        private void InitOnlineTimer()
        {
            ResetOnlineTimer();
        }

        // ���ü�ʱ��
        private void ResetOnlineTimer()
        {
            onlineTimer = c_onlineTimer;
        }

        private void OnlineTimer()
        {
            if (RealNameStatus == RealNameStatus.NotNeed)
                return;
            if (RealNameStatus == RealNameStatus.IsRealName && isAdult == true)
                return;
            if (onlineTimer >= 0)
            {
                onlineTimer -= Time.deltaTime;
                if (onlineTimer < 0)
                {
                    // ѯ�ʷ����� �Ƿ񳬳�����ʱ���δ�����Ƿ�ʱ
                    AskServerOnlineTime();
                    // ���ü�ʱ��
                    ResetOnlineTimer();
                }
            }
        }

        // ���ʵ���ܿ���
        private void TestRealNameSwitch()
        {
            string l_openRealName = SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_OpenRealName, "false");
            openRealName = (l_openRealName == "true");  // �ش�����߿����ܿ���
            Debug.Log("openRealName" + openRealName);
        }

        // ���ʵ����״̬��������ʵ������֤��
        private void TestRealNameStatus()
        {
            RealNameStatus = GetRealNameStatusFromSDK();
            isAdult = SDKManager.IsAdult();
            // �ϱ�������
            AskServerOnlineTime();
        }

        // ѯ�ʷ���������ʱ�䣬�������
        private void AskServerOnlineTime()
        {
            // δʵ���ƣ���ѯ��һ��sdk
            if (RealNameStatus != RealNameStatus.IsRealName || !isAdult)
            {
                RealNameStatus = GetRealNameStatusFromSDK();
                isAdult = SDKManager.IsAdult();
            }
            Debug.LogWarning("AskServerOnlineTime" + RealNameStatus + isAdult);
            RequestRealNameState2Server.RequestRealName(RealNameStatus, isAdult);
        }

        // ��ӷ�������Ϣ����
        private void AddNetEvent()
        {
            GlobalEvent.AddTypeEvent<RequestRealNameState2Client>(OnRequestRealNameResult);
        }

        // ѯ��ʵ���Ƽ��Ļص�
        private void OnRequestRealNameResult(RequestRealNameState2Client e, object[] args)
        {
            RealNameLimitEvent.Dispatch(e.onlineTime, e.night, e.canPlay, e.realNameStatus, e.adult);
        }

        // ����Ƿ�֧������   PayLimitCallBack
        public void CheckPayLimitBySDK(int payAmont)
        {
            if (!openRealName)
                return;
            SDKManager.CheckPayLimit(payAmont);
        }

        // ��õ�ǰʵ����״̬ ���ӻ����У�
        private RealNameStatus GetRealNameStatus()
        {
            if (!openRealName)
            {
                return RealNameStatus.NotNeed;
            }
            else
            {
                return RealNameStatus;
            }
        }

        // ��SDK ��ȡʵ����״̬��������
        private RealNameStatus GetRealNameStatusFromSDK()
        {
            if (!openRealName)
            {
                RealNameStatus = RealNameStatus.NotNeed;
            }
            else
            {
                RealNameStatus = SDKManager.GetRealNameType();
            }
            Debug.Log("GetRealNameStatusFromSDK :openRealName " + openRealName + " realNameStatus:" + RealNameStatus);
            return RealNameStatus;
        }
    }
}