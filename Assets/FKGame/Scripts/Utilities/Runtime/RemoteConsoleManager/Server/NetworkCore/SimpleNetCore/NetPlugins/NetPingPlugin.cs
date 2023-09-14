using System.Threading;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    // Ping功能(客户端)
    public class NetPingPlugin : INetMsgProcessPackestPluginBase
    {
        private EndianBitConverter bitConverter;
        private bool enable = true;                 // 开关Ping
        private bool isInit = false;
        private bool isConnect = false;
        private Session session;
        private Thread pingThread = null;
        private int pingDelayTime = 1000;
        private int ping = -1;
        private byte property = (byte)NetProperty.Ping;

        internal int Ping
        {
            get
            {
                return ping;
            }
        }

        internal bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                enable = value;
                if (isInit && enable)
                {
                    EnableFunc();
                }
                else
                {
                    DisableFunc();
                }
            }
        }
        
        private void EnableFunc()
        {
            if (pingThread == null)
            {
                pingThread = new Thread(ClientPingUpdate);
                pingThread.Start();
                pingThread.IsBackground = true;
            }
        }

        private void DisableFunc()
        {
            if (pingThread != null)
            {
                pingThread.Abort();
                pingThread = null;
            }
        }

        public override byte GetNetProperty()
        {
            return (byte)NetProperty.Pong;
        }

        protected override void OnInit()
        {
            bitConverter = EndianBitConverter.GetBitConverter(networkCommon.Configuration.byteOrder);
            isInit = true;
            if (enable)
            {
                EnableFunc();
            }
        }

        public override void Release()
        {
            isInit = false;
            DisableFunc();
        }

        public override void PeerConnectedEvent(Session session)
        {
            isConnect = true;
            this.session = session;
        }

        public override void DisconnectedEvent(Session session, EDisconnectInfo disconnectInfo)
        {
            isConnect = false;
        }

        // ping发送间隔时间 毫秒
        public void SetPingDelayTime(int time)
        {
            if (time <= 0.1f)
                return;
            pingDelayTime = time;
        }

        private void ClientPingUpdate(object obj)
        {
            int tempTime = pingDelayTime;
            while (true)
            {
                if (enable && isConnect)
                {
                    if (msgQueue.Count > 0)
                    {
                        MsgPackest eventData = msgQueue.Dequeue();
                        long lastTime = bitConverter.ToInt64(eventData.contents, 0);
                        ping = (int)((DateTime.Now.Ticks - lastTime) / 20000);
                        msgQueue.Clear();
                    }
                    if (tempTime <= 0)
                    {
                        tempTime = pingDelayTime;
                        byte[] contents = bitConverter.GetBytes(DateTime.Now.Ticks);
                        byte[] sendBytes = MsgPackest.Write2Bytes(networkCommon.Configuration.byteOrder, 0, 0, 0, property, contents);
                        session.StatisticSendPackets(property, sendBytes.Length);
                        networkCommon.Sendbytes(session, sendBytes);
                    }
                    else
                    {
                        tempTime -= 50;
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}