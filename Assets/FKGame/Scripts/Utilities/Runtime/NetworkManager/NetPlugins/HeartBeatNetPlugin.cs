using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{

    public class HeartBeatNetPlugin : INetPluginBase
    {
        public const string c_HeartBeatMT = "HB";
        public const int ReciveThreadSleepTime = 250;   // �����߳�ѭ�����ʱ�䣨���룩
        public const int SendThreadSleepTime = 250;     // �����߳�ѭ�����ʱ�䣨���룩
        private float m_sendHeatBeatTimer;
        private float m_receviceHeatBeatTimer;

        private Thread reciveHBThread;                  // ������������Ϣ�߳�
        private Thread sendHBThread;                    // �����������߳�
        // ��������Ϣ
        private List<NetworkMessage> s_messageListHeartBeat = new List<NetworkMessage>();

        private float m_heatBeatSendSpaceTime = 3000f;  // �������ͼ��ʱ�䣨���룩
        public float HeatBeatSendSpaceTime
        {
            get
            {
                return m_heatBeatSendSpaceTime;
            }
            set
            {
                m_heatBeatSendSpaceTime = value;
                if (m_heatBeatSendSpaceTime < 0)
                    m_heatBeatSendSpaceTime = 3000;
                ResetSendTimer();
            }
        }

        public override void Init(params object[] paramArray)
        {
            if (paramArray.Length > 0)
            {
                HeatBeatSendSpaceTime = (int)paramArray[0];
            }
            ResetReceviceTimer();
            ResetSendTimer();

            reciveHBThread = new Thread(ReciveHBDealThread);
            reciveHBThread.Start();
            sendHBThread = new Thread(SendHBDealThread);
            sendHBThread.Start();
        }

        private void SendHBDealThread()
        {
            while (true)
            {
                if (s_network.IsConnect)
                {
                    // ��ʱ����������
                    if (m_sendHeatBeatTimer <= 0)
                    {
                        ResetSendTimer();
                        SendHeartBeatMessage();
                    }
                    else
                    {
                        m_sendHeatBeatTimer -= SendThreadSleepTime;
                    }
                }
                else
                {
                    ResetSendTimer();
                }
                Thread.Sleep(SendThreadSleepTime);
            }
        }

        private void ReciveHBDealThread()
        {
            while (true)
            {
                if (s_network.IsConnect)
                {
                    if (GetHeartBeatMessage())
                    {
                        ResetReceviceTimer();
                    }
                    else
                    {
                        m_receviceHeatBeatTimer -= ReciveThreadSleepTime;
                    }
                    // ����û�յ�������������Ϊ����
                    if (m_receviceHeatBeatTimer <= 0)
                    {
                        Debug.Log("HeartBeat Break connect");
                        NetworkManager.DisConnect();
                    }
                }
                else
                {
                    ResetReceviceTimer();
                }
                Thread.Sleep(ReciveThreadSleepTime);
            }
        }

        public override void OnReceiveMsg(NetworkMessage message)
        {
            if (IsHeartBeatMessage(message))
            {
                lock (s_messageListHeartBeat)
                {
                    s_messageListHeartBeat.Add(message);
                }
            }
        }

        // ȡ��������Ϣ
        public bool GetHeartBeatMessage()
        {
            NetworkMessage msg = default(NetworkMessage);
            lock (s_messageListHeartBeat)
            {
                if (s_messageListHeartBeat.Count > 0)
                {
                    msg = s_messageListHeartBeat[0];
                    s_messageListHeartBeat.RemoveAt(0);
                    return true;
                }
            }
            return false;
        }

        public override void OnDispose()
        {
            if (reciveHBThread != null)
            {
                reciveHBThread.Abort();
            }
            if (sendHBThread != null)
            {
                sendHBThread.Abort();
            }
            reciveHBThread = null;
            sendHBThread = null;
        }

        // �ж���Ϣ�Ƿ�����������Ϣ
        public virtual bool IsHeartBeatMessage(NetworkMessage msg)
        {
            if (msg.m_MessageType == null)
                return false;
            if (msg.m_MessageType == c_HeartBeatMT)
            {
                return true;
            }
            return false;
        }

        protected virtual void SendHeartBeatMessage()
        {
            s_network.SendMessage(c_HeartBeatMT, new Dictionary<string, object>());
        }

        // ��������������Timer
        void ResetReceviceTimer()
        {
            m_receviceHeatBeatTimer = HeatBeatSendSpaceTime * 2 + 1000;
        }

        void ResetSendTimer()
        {
            m_sendHeatBeatTimer = HeatBeatSendSpaceTime;
        }
    }
}