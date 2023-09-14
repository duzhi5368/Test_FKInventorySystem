using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // �Զ���������
    public class AutoReconnectController
    {
        private static bool m_Open = true;          // �������ر��Զ�����
        public static bool Open
        {
            get { return m_Open; }
            set
            {
                m_Open = value;
            }
        }

        public static float DelayTime               // ����೤ʱ����������
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
        public static CallBack StartReconnect;      // ��ʼ����
        public static CallBack EndReconnect;        // ��������
        private static bool isBreakConenct = false; // �Ƿ��Ƕ���״̬
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
            // ����
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
            // ���ӳɹ�
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