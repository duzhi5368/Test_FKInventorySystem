using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 自动重连网络
    public class AutoReconnectController
    {
        private static bool m_Open = true;          // 开启，关闭自动重连
        public static bool Open
        {
            get { return m_Open; }
            set
            {
                m_Open = value;
            }
        }

        public static float DelayTime               // 间隔多长时间重新连接
        {
            get
            {
                return delayTime;
            }

            set
            {
                delayTime = value;
                if (delayTime < 1)
                    delayTime = 1;
            }
        }

        private static float delayTime = 3;
        public static CallBack StartReconnect;      // 开始重连
        public static CallBack EndReconnect;        // 结束重连
        private static bool isBreakConenct = false; // 是否是断线状态
        private static bool startReconenct = false;
        private static float tempTimer;
        private static bool isInit = false;

        public static void Init()
        {
            if (isInit)
                return;
            isInit = true;
            InputManager.AddListener<InputNetworkConnectStatusEvent>(OnNetworkConenctStatus);
            ApplicationManager.s_OnApplicationUpdate += Update;
        }

        private static void Update()
        {
            if (!Open)
                return;
            if (startReconenct)
            {
                if (tempTimer <= 0)
                {
                    tempTimer = delayTime;
                    NetworkManager.Connect();
                }
                else
                {
                    tempTimer -= Time.unscaledDeltaTime;
                }
            }
        }

        private static void OnNetworkConenctStatus(InputNetworkConnectStatusEvent msg)
        {
            Debug.Log("OnNetworkConenctStatus :" + msg.m_status);
            // 断线
            if (msg.m_status == NetworkState.FaildToConnect || msg.m_status == NetworkState.ConnectBreak)
            {
                if (!isBreakConenct)
                {
                    isBreakConenct = true;
                    Debug.LogWarning("OnNetworkConenctStatus :" + msg.m_status + " " + isBreakConenct);
                    startReconenct = true;
                    if (Open && StartReconnect != null)
                    {
                        StartReconnect();
                    }
                }
                else
                {
                    Debug.LogWarning("OnNetworkConenctStatus :" + msg.m_status + " " + isBreakConenct);
                }
            }
            // 连接成功
            else if (msg.m_status == NetworkState.Connected)
            {
                isBreakConenct = false;
                startReconenct = false;
                if (Open && EndReconnect != null)
                {
                    EndReconnect();
                }
            }
        }
    }
}