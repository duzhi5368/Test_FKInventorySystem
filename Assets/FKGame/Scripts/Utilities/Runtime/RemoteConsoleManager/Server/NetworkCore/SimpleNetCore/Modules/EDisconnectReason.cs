namespace FKGame
{
    /// <summary>
    /// Disconnect reason that you receive in OnPeerDisconnected event
    /// </summary>
    public enum EDisconnectReason
    {
        ConnectionFailed,
        Timeout,                    // 心跳包超时
        HostUnreachable,
        NetworkUnreachable,
        RemoteConnectionClose,
        DisconnectPeerCalled,       // 本地主动断开连接
        ConnectionRejected,
        InvalidProtocol,
        UnknownHost,
        Reconnect
    }
}