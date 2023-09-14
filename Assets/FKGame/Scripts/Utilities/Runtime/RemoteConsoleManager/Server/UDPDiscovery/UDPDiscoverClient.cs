using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UDPDiscoverClient
    {
        public const string UDPKey = "UDPDiscover";
        private UdpClient client;
        private byte[] RequestData;
        private IPEndPoint broadcastIP;
        private Queue<UDPPackData> packDatas = new Queue<UDPPackData>();
        private Thread sendThread;
        private int port;
        private NetworkInfo network;

        public void Start(NetworkInfo network, int port)
        {
            this.network = network;
            this.port = port;
            var localEndpoint = new IPEndPoint(IPAddress.Parse(network.IPAddress), 0);
            client = new UdpClient(localEndpoint);
            client.EnableBroadcast = true;
            broadcastIP = new IPEndPoint(IPAddress.Broadcast, port);
            client.BeginReceive(Received, client);
            sendThread = new Thread(BackGroudSend);
            sendThread.Start();
            sendThread.IsBackground = true;
        }

        public void Close()
        {
            NetDebug.Log("UDPDiscoverClient.Close");
            if (client != null)
            {
                client.Close();
                client.Dispose();
                client = null;
            }
            if (sendThread != null)
            {
                sendThread.Abort();
                sendThread = null;
            }
        }

        // 异步接收UDP数据
        void Received(IAsyncResult iar)
        {
            client = iar.AsyncState as UdpClient;
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
            byte[] buffer = client.EndReceive(iar, ref ipEndPoint);
            // 将获取的byte[]数据转换成字符串
            var ServerResponse = Encoding.UTF8.GetString(buffer);
            packDatas.Enqueue(new UDPPackData(ipEndPoint, ServerResponse));
            if (packDatas.Count > 10000)
            {
                packDatas.Clear();
            }
            // 继续异步接收数据
            client.BeginReceive(Received, client);
        }

        private void BackGroudSend(object obj)
        {
            while (true)
            {
                Send(UDPKey);
                Thread.Sleep(700);
            }
        }

        public bool GetMessage(out UDPPackData data)
        {
            if (packDatas.Count > 0)
            {
                data = packDatas.Dequeue();
                return true;
            }
            else
            {
                data = default(UDPPackData);
            }
            return false;
        }

        private void Send(string content)
        {
            if (client == null)
                return;
            RequestData = Encoding.UTF8.GetBytes(content);
            client.Send(RequestData, RequestData.Length, broadcastIP);
        }
    }
}