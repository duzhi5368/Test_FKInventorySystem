using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace FKGame
{
    public class NetworkManager
    {
        private static INetwork s_network;
        public static IHeartBeatBase s_heatBeat;
        private static int msgCount = 0;
        private static List<NetworkState> s_statusList = new List<NetworkState>();
        private static List<NetworkMessage> s_messageList = new List<NetworkMessage>();
        private static List<NetworkMessage> s_messageListHeartBeat = new List<NetworkMessage>(); // ��������Ϣ
        private const int MaxDealCount = 2000;

        public static bool IsConnect
        {
            get
            {
                if (s_network == null)
                {
                    return false;
                }
                return s_network.m_socketService.isConnect;
            }
        }

        /// <summary>
        /// �����ʼ��
        /// </summary>
        /// <typeparam name="TProtocol">Э�鴦����</typeparam>
        /// <typeparam name="TSocket">Socket��</typeparam>
        /// <param name="protocolType">ͨѶЭ��</param>
        public static void Init<TProtocol, TSocket>(ProtocolType protocolType = ProtocolType.Tcp) where TProtocol : INetwork, new() where TSocket : ISocketBase, new()
        {
            Init<TProtocol, TSocket>(null, protocolType);
        }

        /// <summary>
        /// �����ʼ��
        /// </summary>
        /// <typeparam name="TProtocol">Э�鴦����</typeparam>
        /// <typeparam name="TSocket">Socket��</typeparam>
        /// <param name="protocolType">ͨѶЭ��</param>
        public static void Init<TProtocol, TSocket>(IMsgCompressBase msgCompress, ProtocolType protocolType = ProtocolType.Tcp) where TProtocol : INetwork, new() where TSocket : ISocketBase, new()
        {
            s_network = new TProtocol();
            s_network.m_socketService = new TSocket();
            s_network.msgCompress = msgCompress;
            Debug.Log("protocolType " + s_network.m_socketService.m_protocolType);
            s_network.m_socketService.m_protocolType = protocolType;
            NetInit();
        }

        public static void Init(string networkInterfaceName, string socketName)
        {
            Type type = Type.GetType(networkInterfaceName);
            s_network = Activator.CreateInstance(type) as INetwork;
            Type socketType = Type.GetType(networkInterfaceName);
            s_network.m_socketService = Activator.CreateInstance(socketType) as ISocketBase;
            s_network.m_socketService.m_protocolType = ProtocolType.Tcp;
            NetInit();
        }

        private static void NetInit()
        {
            // ��ǰ���������¼��ɷ����������첽��ͻ
            InputManager.LoadDispatcher<InputNetworkConnectStatusEvent>();
            InputManager.LoadDispatcher<InputNetworkMessageEvent>();
            s_network.m_socketService.m_byteCallBack = s_network.SpiltMessage;
            s_network.m_socketService.m_connectStatusCallback = ConnectStatusChange;
            s_network.m_socketService.Init();
            s_network.Init();
            s_network.m_messageCallBack = ReceviceMeaasge;
            ApplicationManager.s_OnApplicationUpdate += Update;
            ApplicationManager.s_OnApplicationQuit += DisConnect;
            ApplicationManager.s_OnApplicationQuit += Dispose;
        }

        public static void InitHeartBeat<T>(int spaceTime = 2500) where T : IHeartBeatBase, new()
        {
            s_heatBeat = new T();
            s_heatBeat.Init(spaceTime);
        }

        public static void Dispose()
        {
            InputManager.UnLoadDispatcher<InputNetworkConnectStatusEvent>();
            InputManager.UnLoadDispatcher<InputNetworkMessageEvent>();
            if (s_network != null)
            {
                s_network.Dispose();
                s_network = null;
            }
            if (s_heatBeat != null)
            {
                s_heatBeat.Dispose();
                s_heatBeat = null;
            }
            ApplicationManager.s_OnApplicationUpdate -= Update;
        }

        public static void SetServer(string IP, int port)
        {
            Debug.Log("Set IP=>" + IP + ":" + port);
            IPAddress address;
            if (IPAddress.TryParse(IP, out address))
            {
                s_network.SetIPAddress(IP, port);
            }
            else
            {
                SetDomain(IP, port);
            }
        }

        public static void SetDomain(string url, int port)
        {
            IPHostEntry IPinfo = Dns.GetHostEntry(url);
            IPAddress[] ipList = IPinfo.AddressList;
            Debug.Log("����������" + ipList[0].ToString());
            s_network.SetIPAddress(ipList[0].ToString(), port);
        }

        public static void Connect()
        {
            s_network.Connect();
        }

        public static void DisConnect()
        {
            Debug.Log("�Ͽ�����");
            s_network.Close();
        }

        public static void SendMessage(byte[] msg)
        {
            s_network.m_socketService.Send(msg);
        }

        public static void SendMessage(string messageType, Dictionary<string, object> data)
        {
            if (IsConnect)
            {
                s_network.SendMessage(messageType, data);
            }
            else
            {
                Debug.LogError("socket δ���ӣ�");
            }
        }

        public static void SendMessage(Dictionary<string, object> data)
        {
            try
            {
                if (IsConnect)
                {
                    if (!data.ContainsKey("MT"))
                    {
                        Debug.LogError("NetworkManager SendMessage Error ����Ϣû�м� MT �ֶΣ�");
                        return;
                    }
                    s_network.SendMessage(data["MT"].ToString(), data);
                }
                else
                {
                    Debug.LogError("socket δ���ӣ�");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("SendMessage Error " + e.ToString());
            }
        }

        static void ReceviceMeaasge(NetworkMessage message)
        {
            if (message.m_MessageType != null)
            {
                if (s_heatBeat.IsHeartBeatMessage(message))
                {
                    lock (s_messageListHeartBeat)
                    {
                        s_messageListHeartBeat.Add(message);
                    }
                }
                else
                {
                    lock (s_messageList)
                    {
                        s_messageList.Add(message);
                    }
                }
                msgCount++;
            }
            else
            {
                Debug.LogError("ReceviceMeaasge m_MessageType is null !");
            }
        }

        static void Dispatch(NetworkMessage msg)
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

        static void ConnectStatusChange(NetworkState status)
        {
            lock (s_statusList)
            {
                s_statusList.Add(status);
            }
        }

        static void Dispatch(NetworkState status)
        {
            InputNetworkEventProxy.DispatchStatusEvent(status);
        }

        // ȡ��������Ϣ
        public static bool GetHeartBeatMessage()
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

        // ����Ϣ�Ĵ��������߳�
        static void Update()
        {
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
            if (s_network != null)
            {
                s_network.Update();
            }
        }
    }
}