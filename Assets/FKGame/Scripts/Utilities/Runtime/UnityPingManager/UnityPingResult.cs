namespace FKGame
{
    public enum ErrorReason
    {
        None,
        NetNotReachable,    // �������
        UnreachableAddress, // �޷����ʵ�ַ
        TimeOut,            // ��ʱ
    }

    public struct UnityPingResult
    {
        public bool isSuccess;
        public ErrorReason errorReason;
        public string host;
        public string ip;
        public int pingTime;
        public UnityPingResult(bool isSuccess, ErrorReason errorReason, string host, string ip, int time)
        {
            this.isSuccess = isSuccess;
            this.errorReason = errorReason;
            this.host = host;
            this.ip = ip;
            this.pingTime = time;
        }
    }
}