namespace FKGame.SimpleTCP
{
    /// <summary>
    /// Disconnect reason that you receive in OnPeerDisconnected event
    /// </summary>
    public enum DisconnectReason
    {
        None,
        ConnectionFailed,

        /// <summary>
        /// ���������Ͽ�����
        /// </summary>
        DisconnectPeerCalled,

    }
}