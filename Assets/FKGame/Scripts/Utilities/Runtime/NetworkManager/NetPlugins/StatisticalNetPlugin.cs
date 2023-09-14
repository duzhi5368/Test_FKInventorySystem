using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public sealed class StatisticalNetPlugin : INetPluginBase
    {
        public NetStatistics statistics = new NetStatistics();

        public override void OnSendMsg(string messageType, Dictionary<string, object> data)
        {
            statistics.PacketsSent++;
        }
        public override void OnReceiveMsg(NetworkMessage message)
        {
            statistics.PacketsReceived++;
        }
    }
}