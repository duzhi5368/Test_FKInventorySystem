namespace FKGame
{
    public class ClientConfiguration : NetConfiguration
    {
        public ClientConfiguration(INetworkTransport transport) : base(transport)
        {
        }

        // Ĭ�Ͽͻ�������
        public static ClientConfiguration NewDefaultConfiguration(INetworkTransport transport, INetMsgSerializer serializer, IMessageHandler messageHandler)
        {
            return (ClientConfiguration)new ClientConfiguration(transport)
                .AddPlugin(new DataTypeMsgProcessPlugin())
                .AddPlugin(new NetHeartBeatPingPlugin())
                .AddPlugin(new NetPingPlugin())
                .AddPlugin(new AutoReconnectPlugin())
                 .AddMsgSerializer(serializer)
                .AddMessageHander(messageHandler)
                .SetByteOrder(ByteOrder.BIG_ENDIAN);
        }

        // �Ƿ�����Ping
        private bool enablePing = false;
        public NetConfiguration EnablePing()
        {
            enablePing = true;
            return this;
        }

        // �Ƿ������Զ�����
        private bool enableReconnect = false;
        public NetConfiguration EnableReconnect()
        {
            enableReconnect = true;
            return this;
        }

        internal AutoReconnectPlugin GetReconnectPlugin()
        {
            AutoReconnectPlugin pingPlugin = null;
            INetMsgProcessPluginBase p = GetPlugin(99);
            if (p != null)
            {
                pingPlugin = (AutoReconnectPlugin)p;
            }
            return pingPlugin;
        }

        internal override void Init(NetworkCommon networkCommon)
        {
            INetMsgProcessPluginBase netMsgProcess = GetPlugin((byte)NetProperty.Pong);
            if (netMsgProcess != null)
            {
                NetPingPlugin pingPlugin = (NetPingPlugin)netMsgProcess;
                pingPlugin.Enable = enablePing;
            }
            AutoReconnectPlugin autoReconnect = GetReconnectPlugin();
            if (autoReconnect != null)
            {
                autoReconnect.Enable = enableReconnect;
            }
            base.Init(networkCommon);
        }
    }
}