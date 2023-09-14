namespace FKGame
{
    public class InputNetworkConnectStatusEvent : IInputEventBase
    {
        public NetworkState m_status;

        public InputNetworkConnectStatusEvent()
        {
            m_status = NetworkState.ConnectBreak;
        }

        public InputNetworkConnectStatusEvent(NetworkState status)
        {
            m_status = status;
        }
    }
}