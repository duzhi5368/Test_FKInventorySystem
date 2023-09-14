namespace FKGame
{
    public enum ErrorReason
    {
        None,
        NetNotReachable,    // 网络故障
        UnreachableAddress, // 无法访问地址
        TimeOut,            // 超时
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