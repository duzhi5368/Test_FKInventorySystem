namespace FKGame
{
    public enum NetProperty : byte
    {
        Data = 0,
        /// <summary>
        /// �������ͻ��˷���
        /// </summary>
        HeartBeatClinetSend = 1,
        /// <summary>
        /// ����������˷���
        /// </summary>
        HeartBeatServerSend = 2,
        /// <summary>
        /// ping����
        /// </summary>
        Ping = 3,
        /// <summary>
        /// �����ping����
        /// </summary>
        Pong = 4,
    }
}