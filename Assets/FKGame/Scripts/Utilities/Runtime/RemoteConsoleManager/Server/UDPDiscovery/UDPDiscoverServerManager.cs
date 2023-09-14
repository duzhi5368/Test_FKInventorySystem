using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UDPDiscoverServerManager
    {
        private List<UDPDiscoverServer> udpDisServer = new List<UDPDiscoverServer>();

        public void Start(int port, string content)
        {
            UDPDiscoverServer server = new UDPDiscoverServer();
            server.Start(port, content);
            udpDisServer.Add(server);
        }

        public void Close()
        {
            foreach (var item in udpDisServer)
            {
                item.Close();
            }
            udpDisServer.Clear();
        }
    }
}