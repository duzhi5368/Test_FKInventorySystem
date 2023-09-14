using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 前端间隔1min询问服务器，根据规则触发实名认证，逻辑层应监听 RealNameLimitEvent 事件以处理实名制规则触发后的逻辑（需要游戏逻辑层处理）
    // 通过调用 CheckPayLimit 方法，启动购买限制认证， 监听CheckPayLimitResultEvent 消息，获取结果 （本步骤已与库的购买逻辑结合，游戏逻辑层不需要处理）
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

        public bool isAdult = false;                                    // 是否是成年人
        private bool openRealName = false;                              // 是否开启实名认证 ，总开关
        private RealNameStatus realNameStatus = RealNameStatus.NotNeed; // 实名状态
        float c_onlineTimer = 1 * 60;                                   // 当日游戏时间检测间隔    1分钟 * 60秒/分钟  
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

        // 是否需要实名认证
        public bool IsOpenRealName()
        {
            return openRealName;
        }

        // 是否是成年人
        public bool IsAdult()
        {
            return isAdult;
        }

        // 开始实名认证
        public void StartRealNameAttestation()
        {
            if (openRealName && RealNameStatus == RealNameStatus.NotRealName)
            {
                SDKManager.StartRealNameAttestation();  // 调用sdk 开始实名认证 
                TestRealNameStatus();                   // 将认证结果上报服务器
            }
            else
            {
                Debug.LogWarning("StartRealNameVerify useless: openRealName is" + openRealName + " realNameStatus is " + RealNameStatus);
            }
        }

        // 检测是否有支付限制  需要监听 CheckPayLimitResultEvent 消息，获取结果
        public void CheckPayLimit(int payAmount)
        {
            CheckPayLimitEvent.Dispatch(payAmount);
        }

        public void Init()
        {
            // 检测总开关
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
                // 检测实名状态（但不触发实名）
                InitOnlineTimer();
            }
            GlobalEvent.AddTypeEvent<CheckPayLimitEvent>(OnCheckPayLimit);
        }

        // 需要登出账号
        private void OnNeedLogout()
        {
            Debug.Log("========RealNameManager OnNeedLogout");
            LoginGameController.Logout();
        }

        private void OnLogoutEvent(UserLogout2Client t)
        {
            SDKManager.RealNameLogout();
            RealNameStatus = RealNameStatus.NotNeed;    // 已经登出，停止检测
        }

        private void OnLoginEvent(UserLogin2Client t)
        {
            Debug.Log("RealNameManager OnLoginEvent" + t.code + t.reloginState);
            if (t.code != 0 || t.reloginState)
                return;
            SDKManager.RealNameLogin(t.user.userID);
            TestRealNameStatus();
        }

        // 查询支付结果的回调
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

        // SDK 实名制认证回调
        private void OnRealNameCallBack(RealNameData info)
        {
            Debug.Log("OnRealNameCallBack" + info.realNameStatus + " isAdult:" + info.isAdult);
            RealNameStatus = info.realNameStatus;
            isAdult = info.isAdult;
        }

        // 接收到询问支付限制的事件
        private void OnCheckPayLimit(CheckPayLimitEvent e, object[] args)
        {
            PayLimitType payLimitType = PayLimitType.None;// 默认不需要实名认证，无限制
            Debug.LogWarning("OnCheckPayLimit====openRealName==" + openRealName);
            if (openRealName)
            {
                if (RealNameStatus == RealNameStatus.NotRealName)
                {
                    StartRealNameAttestation(); // 自动开始实名制认证
                    payLimitType = PayLimitType.NoRealName;
                }
                else if (RealNameStatus == RealNameStatus.IsRealName)
                {
                    if (isAdult) // 成年
                    {
                        payLimitType = PayLimitType.None;
                    }
                    else // 未成年
                    {
                        CheckPayLimitBySDK(e.payAmount);
                        Debug.LogWarning("CheckPayLimitBySDK");
                        return;
                    }
                }
                else if (RealNameStatus == RealNameStatus.NotNeed)
                {
                    payLimitType = PayLimitType.None;// 默认不需要实名认证，无限制
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

        // 重置计时器
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
                    // 询问服务器 是否超出体验时间或未成年是否超时
                    AskServerOnlineTime();
                    // 重置计时器
                    ResetOnlineTimer();
                }
            }
        }

        // 检测实名总开关
        private void TestRealNameSwitch()
        {
            string l_openRealName = SDKManager.GetProperties(SDKInterfaceDefine.PropertiesKey_OpenRealName, "false");
            openRealName = (l_openRealName == "true");  // 重打包工具控制总开关
            Debug.Log("openRealName" + openRealName);
        }

        // 检测实名制状态（不触发实名制认证）
        private void TestRealNameStatus()
        {
            RealNameStatus = GetRealNameStatusFromSDK();
            isAdult = SDKManager.IsAdult();
            // 上报服务器
            AskServerOnlineTime();
        }

        // 询问服务器在线时间，触发检测
        private void AskServerOnlineTime()
        {
            // 未实名制，再询问一下sdk
            if (RealNameStatus != RealNameStatus.IsRealName || !isAdult)
            {
                RealNameStatus = GetRealNameStatusFromSDK();
                isAdult = SDKManager.IsAdult();
            }
            Debug.LogWarning("AskServerOnlineTime" + RealNameStatus + isAdult);
            RequestRealNameState2Server.RequestRealName(RealNameStatus, isAdult);
        }

        // 添加服务器消息监听
        private void AddNetEvent()
        {
            GlobalEvent.AddTypeEvent<RequestRealNameState2Client>(OnRequestRealNameResult);
        }

        // 询问实名制检测的回调
        private void OnRequestRealNameResult(RequestRealNameState2Client e, object[] args)
        {
            RealNameLimitEvent.Dispatch(e.onlineTime, e.night, e.canPlay, e.realNameStatus, e.adult);
        }

        // 检测是否支付限制   PayLimitCallBack
        public void CheckPayLimitBySDK(int payAmont)
        {
            if (!openRealName)
                return;
            SDKManager.CheckPayLimit(payAmont);
        }

        // 获得当前实名制状态 （从缓存中）
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

        // 从SDK 获取实名制状态，并缓存
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