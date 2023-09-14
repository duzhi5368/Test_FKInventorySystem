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

        // ����IP�Ͷ˿�
        public virtual void SetIPAddress(string IP, int port)
        {
            m_IPaddress = IP;
            m_port = port;
            endPoint = new IPEndPoint(IPAddress.Parse(m_IPaddress), port);
        }

        // ��������
        public abstract void Send(byte[] sendbytes);

        // ��������
        public abstract void Connect();

        // �ر�����
        public abstract void Close();

        // ֡����
        public abstract void Update();
    }
}