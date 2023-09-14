using System.Net.NetworkInformation;
using System.Net;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class PingNetPlugin : INetPluginBase
    {
        public long Ping { get; private set; }
        private IPEndPoint remoteIPEndPort;

        public override void Update()
        {
            PingLogic();
        }

        void PingLogic()
        {
            if (s_network == null)
            {
                Debug.Log("s_network is null");
                return;
            }
            remoteIPEndPort = s_network.RemoteIPEndPort;
            var sender = new System.Net.NetworkInformation.Ping();
            var reply = sender.Send(remoteIPEndPort.Address);
            if (reply.Status == IPStatus.Success)
            {
                Ping = reply.RoundtripTime;
            }
        }
    }
}