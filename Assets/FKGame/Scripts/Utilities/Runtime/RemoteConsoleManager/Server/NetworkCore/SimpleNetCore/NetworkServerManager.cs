
//------------------------------------------------------------------------
namespace FKGame
{
    public class NetworkServerManager : NetworkCommon
    {
        public System.Action<Session> OnPeerConnected;
        public System.Action<Session, EDisconnectInfo> OnPeerDisconnected;
        public System.Action OnStopServer;

        public NetworkServerManager(ServerConfiguration configuration) : base(configuration)
        {
        }

        protected override void OnDisconnectedEvent(Session session, EDisconnectInfo disconnectInfo)
        {
            if (OnPeerDisconnected != null)
                OnPeerDisconnected(session, disconnectInfo);
        }
        protected override void PeerConnectedEvent(Session session)
        {

            if (OnPeerConnected != null)
                OnPeerConnected(session);
        }

        public void Start(int port)
        {
            NetStart(port);
        }

        protected override void OnStopEvent()
        {
            NetDebug.Log(" NetworkServerManager::OnStopServer");
            if (OnStopServer != null)
                OnStopServer();
        }

        public void Send<T>(Session session, T messageData)
        {
            SendData(session, null, messageData);
        }
    }
}