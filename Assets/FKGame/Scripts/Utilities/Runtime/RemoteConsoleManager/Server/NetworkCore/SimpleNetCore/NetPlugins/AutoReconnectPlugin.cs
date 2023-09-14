using System.Threading;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AutoReconnectPlugin : INetMsgProcessPluginBase
    {
        public override byte GetNetProperty()
        {
            return 99;
        }
        private Thread reconnectThread;
        private bool isRelease = false;
        private bool enable = true;
        private int updateTime = 3500;
        private int tempTime = 0;
        public bool IsReconnecting { get; private set; }
        public bool Enable
        {
            get => enable; set
            {
                enable = value;
                if (enable == false)
                {
                    ForceStopReconnect();
                }
            }
        }
        public override void Release()
        {
            isRelease = true;
            ForceStopReconnect();
        }

        protected override void OnInit()
        {
            isRelease = false;
        }

        public void ForceStopReconnect()
        {
            IsReconnecting = false;
            if (reconnectThread != null)
            {
                reconnectThread.Abort();
                reconnectThread = null;
            }
        }

        public override void PeerConnectedEvent(Session session)
        {
            ForceStopReconnect();
        }

        public override void DisconnectedEvent(Session session, EDisconnectInfo disconnectInfo)
        {
            if (isRelease)
                return;
            if (IsReconnecting && disconnectInfo.Reason == EDisconnectReason.DisconnectPeerCalled)
            {
                ForceStopReconnect();
                return;
            }
            if (Enable && !IsReconnecting && disconnectInfo.Reason != EDisconnectReason.DisconnectPeerCalled)
            {
                NetDebug.Log("DisconnectedEvent ��ʼ����");
                IsReconnecting = true;
                tempTime = 0;
                reconnectThread = new Thread(ReconnectThreadFun);
                reconnectThread.Start();
                reconnectThread.IsBackground = true;
            }
        }

        private void ReconnectThreadFun(object obj)
        {
            while (true)
            {
                ReconnectUpdate(500);
                Thread.Sleep(500);
            }
        }

        private void ReconnectUpdate(int deltaTime)
        {

            if (tempTime < 0)
            {
                tempTime = updateTime;
                NetDebug.Log("��ʼ��������:" + networkCommon.networkAddress + ":" + networkCommon.networkPort);
                networkCommon.Configuration.Transport.Connect(networkCommon.networkAddress, networkCommon.networkPort);
            }
            else
            {
                tempTime -= deltaTime;
            }
        }
    }
}