using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class IHeartBeatBase
    {
        public const string c_HeartBeatMT = "HB";
        public const int ReciveThreadSleepTime = 250;   // 接收线程循环间隔时间（毫秒）
        public const int SendThreadSleepTime = 250;     // 发送线程循环间隔时间（毫秒）

        private float m_sendHeatBeatTimer;
        private float m_receviceHeatBeatTimer;

        private Thread reciveHBThread;                  // 接收心跳包消息线程
        private Thread sendHBThread;                    // 发送心跳包线程

        private float m_heatBeatSendSpaceTime = 3000f;  // 心跳包发送间隔时间（毫秒）
        public float HeatBeatSendSpaceTime
        {
            get{ return m_heatBeatSendSpaceTime; }
            set
            {
                m_heatBeatSendSpaceTime = value;
                if (m_heatBeatSendSpaceTime < 0)
                    m_heatBeatSendSpaceTime = 3000;
                ResetSendTimer();
            }
        }

        public virtual void Init(int spaceTime)
        {
            HeatBeatSendSpaceTime = spaceTime;
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
                if (NetworkManager.IsConnect)
                {
                    // 定时发送心跳包
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
                if (NetworkManager.IsConnect)
                {
                    if (NetworkManager.GetHeartBeatMessage())
                    {
                        ResetReceviceTimer();
                    }
                    else
                    {
                        m_receviceHeatBeatTimer -= ReciveThreadSleepTime;
                    }
                    // 长期没收到服务器返回认为断线
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

        public virtual void Dispose()
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

        // 判断消息是否是心跳包消息
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
            NetworkManager.SendMessage(c_HeartBeatMT, new Dictionary<string, object>());
        }


        // 重置心跳包接收Timer
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