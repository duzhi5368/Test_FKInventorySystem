using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UDPDiscoverServer
    {
        private UdpClient udpServer;
        private byte[] ResponseData;
        private int port;

        public void Start(int port, string content)
        {
            ResponseData = Encoding.UTF8.GetBytes(content);
            this.port = port;
            udpServer = new UdpClient(port);
            udpServer.EnableBroadcast = true;
            udpServer.BeginReceive(Received, udpServer);
        }

        // 异步接收UDP数据
        void Received(IAsyncResult iar)
        {
            udpServer = iar.AsyncState as UdpClient;
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port); ;
            byte[] buffer = udpServer.EndReceive(iar, ref ipEndPoint);
            var ClientRequest = Encoding.UTF8.GetString(buffer);
            if (ClientRequest == UDPDiscoverClient.UDPKey)
            {
                udpServer.Send(ResponseData, ResponseData.Length, ipEndPoint);
            }
            // 继续异步接收数据
            udpServer.BeginReceive(Received, udpServer);
        }

        public void Close()
        {
            if (udpServer != null)
            {
                udpServer.Close();
                udpServer.Dispose();
                udpServer = null;
            }
        }
    }
}