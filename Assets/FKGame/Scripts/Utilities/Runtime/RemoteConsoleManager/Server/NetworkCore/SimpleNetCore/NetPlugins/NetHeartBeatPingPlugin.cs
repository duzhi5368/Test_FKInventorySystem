using System.Threading;
//------------------------------------------------------------------------
namespace FKGame
{
    public class NetHeartBeatPingPlugin : INetMsgProcessPackestPluginBase
    {
        private byte[] sendBytes = null;
        private bool isConnect = false;
        private Session session;
        private Thread heardBeatThread = null;
        private int heatBeatTime = 7000;
        private const int ThreadUpdateTime = 500;
        private int tempTime = 1000;
        private int lostCount = 0;

        public override byte GetNetProperty()
        {
            return (byte)NetProperty.HeartBeatServerSend;
        }

        protected override void OnInit()
        {
            if (networkCommon.Configuration.UseMultithreading)
            {
                heardBeatThread = new Thread(HeardBeatThreadFun);
                heardBeatThread.Start();
                heardBeatThread.IsBackground = true;
            }
            sendBytes = MsgPackest.Write2Bytes(networkCommon.Configuration.byteOrder,
                0, 0, 0, (byte)NetProperty.HeartBeatClinetSend, new byte[0]);
        }

        public override void Release()
        {
            if (heardBeatThread != null)
            {
                heardBeatThread.Abort();
                heardBeatThread = null;
            }
        }

        public override void PeerConnectedEvent(Session session)
        {
            isConnect = true;
            this.session = session;
            ResetFlag();
        }

        public override void DisconnectedEvent(Session session, EDisconnectInfo disconnectInfo)
        {
            isConnect = false;
        }

        // 心跳间隔时间 毫秒
        public void SetheatBeatTime(int time)
        {
            if (time <= 0)
                return;
            heatBeatTime = time;
        }

        private void HeardBeatThreadFun(object obj)
        {
            while (true)
            {
                ClientHeardBeatUpdate(ThreadUpdateTime);
                Thread.Sleep(ThreadUpdateTime);
            }
        }

        public override void Update(float deltaTime)
        {
            if (!networkCommon.Configuration.UseMultithreading)
            {
                int dt = (int)(deltaTime * 1000);
                ClientHeardBeatUpdate(dt);
            }
        }

        private void ResetFlag()
        {
            msgQueue.Clear();
            lostCount = 0;
            tempTime = heatBeatTime;
        }

        private void ClientHeardBeatUpdate(int deltaTime)
        {
            if (isConnect)
            {
                if (msgQueue.Count > 0)
                {
                    ResetFlag();
                    return;
                }
                if (tempTime <= 0)
                {
                    lostCount++;
                    tempTime = heatBeatTime + lostCount * 2;
                    if (lostCount > 4)
                    {
                        NetDebug.Log("ClientHeardBeatUpdate Disconnect! lostCount:" + lostCount);
                        networkCommon.Configuration.Transport.Disconnect(this.session.ConnectionId, EDisconnectReason.Timeout);
                        isConnect = false;
                        lostCount = 0;
                        return;
                    }
                    else
                    {
                        session.StatisticSendPackets((byte)NetProperty.HeartBeatClinetSend, sendBytes.Length);
                        networkCommon.Sendbytes(session, sendBytes);
                    }
                }
                else
                {
                    tempTime -= deltaTime;
                }
            }
        }
    }
}