namespace FKGame
{
    /// <summary>
    /// Disconnect reason that you receive in OnPeerDisconnected event
    /// </summary>
    public enum EDisconnectReason
    {
        ConnectionFailed,
        Timeout,                    // ��������ʱ
        HostUnreachable,
        NetworkUnreachable,
        RemoteConnectionClose,
        DisconnectPeerCalled,       // ���������Ͽ�����
        ConnectionRejected,
        InvalidProtocol,
        UnknownHost,
        Reconnect
    }
}