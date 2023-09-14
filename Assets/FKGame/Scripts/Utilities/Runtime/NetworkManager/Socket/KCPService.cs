using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class KCPService : ISocketBase
    {
        private static readonly DateTime utc_time = new DateTime(1970, 1, 1);

        private const UInt32 CONNECT_TIMEOUT = 5000;
        private const UInt32 RESEND_CONNECT = 500;

        private UdpClient mUdpClient;
        private IPEndPoint mIPEndPoint;
        private IPEndPoint mSvrEndPoint;

        private KCP mKcp;
        private bool mNeedUpdateFlag;
        private UInt32 mNextUpdateTime;
        private UInt32 mConnectStartTime;
        private UInt32 mLastSendConnectTime;

        private SwitchQueue<byte[]> mRecvQueue = new SwitchQueue<byte[]>(128);

        public static UInt32 iclock()
        {
            return (UInt32)(Convert.ToInt64(DateTime.UtcNow.Subtract(utc_time).TotalMilliseconds) & 0xffffffff);
        }

        public override void Connect()
        {
            Debug.Log("Connect " + m_IPaddress + " : " + m_port);
            mSvrEndPoint = new IPEndPoint(IPAddress.Parse(m_IPaddress), m_port);
            mUdpClient = new UdpClient(m_IPaddress, m_port);
            m_connectStatusCallback(NetworkState.Connecting);
            try
            {
                mUdpClient.Connect(mSvrEndPoint);
                isConnect = true;
                m_connectStatusCallback(NetworkState.Connected);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            reset_state();
            init_kcp(1);
            mConnectStartTime = iclock();
            mUdpClient.BeginReceive(ReceiveCallback, this);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            Byte[] data = (mIPEndPoint == null) ?
                mUdpClient.Receive(ref mIPEndPoint) :
                mUdpClient.EndReceive(ar, ref mIPEndPoint);
            if (null != data)
                OnData(data);
            if (mUdpClient != null)
            {
                mUdpClient.BeginReceive(ReceiveCallback, this);
            }
        }

        void OnData(byte[] buf)
        {
            mRecvQueue.Push(buf);
        }

        void reset_state()
        {
            mNeedUpdateFlag = false;
            mNextUpdateTime = 0;
            mConnectStartTime = 0;
            mLastSendConnectTime = 0;
            mRecvQueue.Clear();
            mKcp = null;
        }

        string dump_bytes(byte[] buf, int size)
        {
            var sb = new StringBuilder(size * 2);
            for (var i = 0; i < size; i++)
            {
                sb.Append(buf[i]);
                sb.Append(" ");
            }
            return sb.ToString();
        }

        void init_kcp(UInt32 conv)
        {
            mKcp = new KCP(conv, (byte[] buf, int size) =>
            {
                try
                {
                    mUdpClient.Send(buf, size);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });
            mKcp.NoDelay(1, 10, 2, 1);
            mKcp.WndSize(128, 128);
        }

        public override void Send(byte[] buf)
        {
            mKcp.Send(buf);
            mNeedUpdateFlag = true;
        }

        public override void Update()
        {
            update(iclock());
        }

        public override void Close()
        {
            mUdpClient.Close();
            m_connectStatusCallback(NetworkState.ConnectBreak);
        }

        void process_recv_queue(UInt32 current)
        {
            mRecvQueue.Switch();
            while (!mRecvQueue.Empty())
            {
                var buf = mRecvQueue.Pop();
                int input = mKcp.Input(buf);
                mNeedUpdateFlag = true;
                for (var size = mKcp.PeekSize(); size > 0; size = mKcp.PeekSize())
                {
                    var buffer = new byte[size];
                    if (mKcp.Recv(buffer) > 0)
                    {
                        int offset = 0;
                        m_byteCallBack(buffer, ref offset, buffer.Length);
                    }
                }
            }
        }

        bool connect_timeout(UInt32 current)
        {
            return current - mConnectStartTime > CONNECT_TIMEOUT;
        }

        bool need_send_connect_packet(UInt32 current)
        {
            return current - mLastSendConnectTime > RESEND_CONNECT;
        }

        void update(UInt32 current)
        {
            if (isConnect)
            {
                process_recv_queue(current);

                if (mNeedUpdateFlag || current >= mNextUpdateTime)
                {
                    mKcp.Update(current);
                    mNextUpdateTime = mKcp.Check(current);
                    mNeedUpdateFlag = false;
                }
            }
        }
    }
}