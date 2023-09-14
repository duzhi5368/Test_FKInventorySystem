using System.Net.Sockets;
using System.Net;
//------------------------------------------------------------------------
namespace FKGame
{
    public delegate void ByteCallBack(byte[] data, ref int offset, int length);
    public delegate void MessageCallBack(NetworkMessage receStr);
    public delegate void ConnectStatusCallBack(NetworkState connectStstus);

    public abstract class ISocketBase
    {
        public string m_IPaddress = "";
        public int m_port = 0;
        public IPEndPoint endPoint;
        public bool isConnect = false;
        public ConnectStatusCallBack m_connectStatusCallback;
        public ByteCallBack m_byteCallBack;
        public ProtocolType m_protocolType;
        public byte[] m_readData = new byte[1024 * 1024];

        public virtual void Init(){}

        public virtual void Dispose() {}

        // 设置IP和端口
        public virtual void SetIPAddress(string IP, int port)
        {
            m_IPaddress = IP;
            m_port = port;
            endPoint = new IPEndPoint(IPAddress.Parse(m_IPaddress), port);
        }

        // 发送数据
        public abstract void Send(byte[] sendbytes);

        // 建立连接
        public abstract void Connect();

        // 关闭连接
        public abstract void Close();

        // 帧更新
        public abstract void Update();
    }
}