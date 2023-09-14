using FKGame.LiteNetLib;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LiteNetLibTransportEventListener : INetEventListener
    {
        private LiteNetLibTransport transport;
        private Queue<TransportEventData> eventQueue;
        private Dictionary<long, NetPeer> peersDict;
        private bool isServer;

        public LiteNetLibTransportEventListener(bool isServer, LiteNetLibTransport transport, Queue<TransportEventData> eventQueue, Dictionary<long, NetPeer> peersDict)
        {
            this.isServer = isServer;
            this.peersDict = peersDict;
            this.transport = transport;
            this.eventQueue = eventQueue;
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.AcceptIfKey(transport.connectKey);
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            byte[] datas = new byte[reader.AvailableBytes];
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = reader.RawData[reader.Position + i];
            }
            eventQueue.Enqueue(new TransportEventData()
            {
                type = ENetworkEvent.DataEvent,
                connectionId = peer.Id,
                data = datas,
            });
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
        }

        public void OnPeerConnected(NetPeer peer)
        {
            if (peersDict != null)
                peersDict[peer.Id] = peer;

            eventQueue.Enqueue(new TransportEventData()
            {
                type = ENetworkEvent.ConnectEvent,
                connectionId = peer.Id,
            });
        }

        public void OnPeerDisconnected(NetPeer peer, LiteNetLib.DisconnectInfo disconnectInfo)
        {
            Debug.Log("OnPeerDisconnected:" + peer.EndPoint);
            if (peersDict != null)
                peersDict.Remove(peer.Id);
            eventQueue.Enqueue(new TransportEventData()
            {
                type = ENetworkEvent.DisconnectEvent,
                connectionId = peer.Id,
                disconnectInfo = new EDisconnectInfo((EDisconnectReason)((int)disconnectInfo.Reason), disconnectInfo.SocketErrorCode),
            });
        }
    }
}