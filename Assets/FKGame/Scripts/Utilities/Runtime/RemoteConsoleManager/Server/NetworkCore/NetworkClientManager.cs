using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class NetworkClientManager : NetworkCommon
    {
        public System.Action OnStopClient;
        private Session session;
        private ClientNetStatistics clientNetStatistics = new ClientNetStatistics();
        public Action<NetConnectState, EDisconnectInfo> OnNetConnectStateChange;            // 连接状态变化
        private NetConnectState m_connectState = NetConnectState.DisConnected;
        private NetPingPlugin pingPlugin;
        private AutoReconnectPlugin autoReconnectPlugin;

        public string NetworkAddress { get { return networkAddress; } }
        public bool IsConnected { get; private set; }
        public Session Session { get { return session; } private set { session = value; } }

        public int Ping
        {
            get
            {
                if (pingPlugin != null)
                    return pingPlugin.Ping;
                return -1;
            }
        }

        // 开关ping
        public bool EnablePing
        {
            get
            {
                if (pingPlugin != null)
                {
                    return pingPlugin.Enable;
                }
                return false;
            }
            set
            {
                if (pingPlugin != null)
                {
                    pingPlugin.Enable = value;
                }
            }
        }
        
        public ClientNetStatistics Statistics
        {
            get
            {
                return clientNetStatistics;
            }
        }

        public NetConnectState ConnectState
        {
            get
            {
                return m_connectState;
            }
        }

        private void SetNetConnectState(NetConnectState m_connectState)
        {
            SetNetConnectState(m_connectState, default(EDisconnectInfo));
        }

        private void SetNetConnectState(NetConnectState m_connectState, EDisconnectInfo info)
        {
            this.m_connectState = m_connectState;
            if (OnNetConnectStateChange != null)
            {
                OnNetConnectStateChange(m_connectState, info);
            }
        }

        public NetworkClientManager(ClientConfiguration configuration) : base(configuration){}

        public void Start()
        {
            NetStart(0);
            INetMsgProcessPluginBase plugin = Configuration.GetPlugin((byte)NetProperty.Pong);
            autoReconnectPlugin = ((ClientConfiguration)Configuration).GetReconnectPlugin();
            if (plugin != null)
            {
                pingPlugin = (NetPingPlugin)plugin;
            }
        }

        public bool Connect()
        {
            return Connect(networkAddress, networkPort);
        }

        public bool Connect(string networkAddress, int networkPort)
        {
            clientNetStatistics = new ClientNetStatistics();
            if (IsConnected)
            {
                NetDebug.LogError("client is connected!");
                return false;
            }
            if (ConnectState == NetConnectState.Connecting)
                return false;
            SetNetConnectState(NetConnectState.Connecting);
            this.networkAddress = networkAddress;
            this.networkPort = networkPort;
            NetDebug.Log("Client connecting to " + networkAddress + ":" + networkPort);
            if (Transport.Connect(networkAddress, networkPort))
            {
                return true;
            }
            SetNetConnectState(NetConnectState.DisConnected);
            return false;
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                NetDebug.Log("Client Disconnect");
                Transport.Disconnect(session.ConnectionId, EDisconnectReason.DisconnectPeerCalled);
            }
            else
            {
                if (autoReconnectPlugin != null)
                {
                    autoReconnectPlugin.ForceStopReconnect();
                }
            }
        }

        protected override void OnStopEvent()
        {
            Disconnect();
            if (OnStopClient != null)
                OnStopClient();
        }

        public bool Send<T>(T messageData)
        {
            if (IsConnected)
                return SendData(session, null, messageData);
            else
            {
                NetDebug.LogError("Client not Connected!");
                return false;
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (autoReconnectPlugin != null)
            {
                if (autoReconnectPlugin.IsReconnecting)
                {
                    if (ConnectState == NetConnectState.Connecting)
                    {
                        return;
                    }
                    SetNetConnectState(NetConnectState.Connecting);
                }
            }
        }

        protected override void OnDisconnectedEvent(Session session, EDisconnectInfo disconnectInfo)
        {
            if (Configuration.UseStatistics && session != null)
            {
                clientNetStatistics.MarkDisconnect();
                clientNetStatistics.details.Add(session.statistics);
            }
            NetDebug.Log("Client DisconnectedEvent :" + session + " disconnectInfo:" + disconnectInfo.Reason);
            IsConnected = false;
            SetNetConnectState(NetConnectState.DisConnected, disconnectInfo);
        }

        protected override void PeerConnectedEvent(Session session)
        {
            NetDebug.Log("Client Peer Connected ! connectionId :" + session);
            if (Configuration.UseStatistics)
            {
                clientNetStatistics.MarkConnected();
                clientNetStatistics.details.Add(session.statistics);
            }
            IsConnected = true;
            this.session = session;
            SetNetConnectState(NetConnectState.Connected);
        }
    }
}