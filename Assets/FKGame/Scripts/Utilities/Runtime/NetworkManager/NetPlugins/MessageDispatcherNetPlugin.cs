using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class MessageDispatcherNetPlugin : INetPluginBase
    {
        private static List<NetworkState> s_statusList = new List<NetworkState>();
        private static List<NetworkMessage> s_messageList = new List<NetworkMessage>();
        const int MaxDealCount = 2000;
        private static int msgCount = 0;

        public override void Init(params object[] paramArray)
        {
            ApplicationManager.s_OnApplicationUpdate += UnityUpdate;
            ApplicationManager.s_OnApplicationQuit += OnApplicationQuit;
            //提前加载网络事件派发器，避免异步冲突
            InputManager.LoadDispatcher<InputNetworkConnectStatusEvent>();
            InputManager.LoadDispatcher<InputNetworkMessageEvent>();
        }

        public override void OnDispose()
        {
            ApplicationManager.s_OnApplicationUpdate -= UnityUpdate;
            ApplicationManager.s_OnApplicationQuit -= OnApplicationQuit;

            InputManager.UnLoadDispatcher<InputNetworkConnectStatusEvent>();
            InputManager.UnLoadDispatcher<InputNetworkMessageEvent>();
        }

        private void OnApplicationQuit()
        {
            if (s_network != null)
            {
                s_network.DisConnect();
                s_network.Dispose();
            }
        }

        public override void OnConnectStateChange(NetworkState status)
        {
            lock (s_statusList)
            {
                s_statusList.Add(status);
            }
        }
        
        public override void OnReceiveMsg(NetworkMessage message)
        {
            if (message.m_MessageType != null)
            {
                lock (s_messageList)
                {
                    s_messageList.Add(message);
                }
                msgCount++;
            }
            else
            {
                Debug.LogError("ReceviceMeaasge m_MessageType is null !");
            }
        }

        private void UnityUpdate()
        {
            if (s_network != null)
            {
                s_network.Update(Time.deltaTime);
            }
            if (s_messageList.Count > 0)
            {
                lock (s_messageList)
                {
                    for (int i = 0; i < s_messageList.Count; i++)
                    {
                        Dispatch(s_messageList[i]);
                        s_messageList.RemoveAt(i);
                        i--;
                    }
                }
            }
            lock (s_statusList)
            {
                if (s_statusList.Count > 0)
                {
                    for (int i = 0; i < s_statusList.Count; i++)
                    {
                        Dispatch(s_statusList[i]);
                    }
                    s_statusList.Clear();
                }
            }
        }

        void Dispatch(NetworkMessage msg)
        {
            try
            {
                InputNetworkEventProxy.DispatchMessageEvent(msg.m_MessageType, msg.m_data);
            }
            catch (Exception e)
            {
                string messageContent = "";
                if (msg.m_data != null)
                {
                    messageContent = MiniJSON.Serialize(msg.m_data);
                }
                Debug.LogError("Message Error: MessageType is ->" + msg.m_MessageType + "<- MessageContent is ->" + messageContent + "<-\n" + e.ToString());
            }
        }

        void Dispatch(NetworkState status)
        {
            InputNetworkEventProxy.DispatchStatusEvent(status);
        }
    }
}