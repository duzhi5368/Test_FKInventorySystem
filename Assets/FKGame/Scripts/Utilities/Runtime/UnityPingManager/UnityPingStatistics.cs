using System.Text;
//------------------------------------------------------------------------
namespace FKGame
{
    public struct UnityPingStatistics
    {
        public string host;
        public string ip;
        public int pingTimes;       // ping����
        public int pingPass;        // pingͨ����
        public int maxPingTime;     // �pingʱ��
        public int minPingTime;     // ���pingʱ��
        public int averagePingTime; // ƽ��pingʱ��

        public UnityPingStatistics(string host, string ip, int pingTimes, int pingPass, int maxPingTime, int minPingTime, int averagePingTime)
        {
            this.host = host;
            this.ip = ip;
            this.pingTimes = pingTimes;
            this.pingPass = pingPass;
            this.maxPingTime = maxPingTime;
            this.minPingTime = minPingTime;
            this.averagePingTime = averagePingTime;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Ping:" + host + "\n");
            builder.Append("Ping Times:" + pingTimes + " Pass Times:" + pingPass + "\n");
            if (pingPass > 0)
            {
                builder.Append("Min Ping:" + minPingTime + "ms Max Ping:" + maxPingTime + "ms AVG:" + averagePingTime + "\n");
            }
            return builder.ToString();
        }
    }
}