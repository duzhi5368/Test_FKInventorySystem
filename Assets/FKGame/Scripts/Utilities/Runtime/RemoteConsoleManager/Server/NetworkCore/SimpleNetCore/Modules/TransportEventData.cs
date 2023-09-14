namespace FKGame
{
    public struct TransportEventData
    {
        public ENetworkEvent type;
        public long connectionId;
        public byte[] data;
        public EDisconnectInfo disconnectInfo;
        internal Session session;
    }
}