using System.Net.Sockets;
//------------------------------------------------------------------------
namespace FKGame
{
    /// <summary>
    /// Additional information about disconnection
    /// </summary>
    public struct EDisconnectInfo
    {
        /// <summary>
        /// Additional info why peer disconnected
        /// </summary>
        public EDisconnectReason Reason;

        /// <summary>
        /// Error code (if reason is SocketSendError or SocketReceiveError)
        /// </summary>
        public SocketError SocketErrorCode;

        public EDisconnectInfo(EDisconnectReason Reason, SocketError SocketErrorCode)
        {
            this.Reason = Reason;
            this.SocketErrorCode = SocketErrorCode;
        }
    }
}