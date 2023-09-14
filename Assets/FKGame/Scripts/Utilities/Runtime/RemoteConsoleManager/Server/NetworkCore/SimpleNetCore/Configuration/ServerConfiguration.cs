namespace FKGame
{
    public class ServerConfiguration : NetConfiguration
    {
        public ServerConfiguration(INetworkTransport transport) : base(transport)
        {
        }

        // ƒ¨»œ∑˛ŒÒ∂À≈‰÷√
        public static ServerConfiguration NewDefaultConfiguration(INetworkTransport transport, INetMsgSerializer serializer, IMessageHandler messageHandler)
        {
            return (ServerConfiguration)new ServerConfiguration(transport)
                .AddPlugin(new DataTypeMsgProcessPlugin())
                .AddPlugin(new NetHeartBeatPongPlugin())
                .AddPlugin(new NetPongPlugin())
                .AddMsgSerializer(serializer)
                .AddMessageHander(messageHandler)
                 .SetByteOrder(ByteOrder.BIG_ENDIAN);
        }
    }
}