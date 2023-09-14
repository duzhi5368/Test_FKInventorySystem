namespace FKGame
{
    // 实名制信息
    public class RealNameData
    {
        public bool canPlay = true;                                     // 是否可以继续游玩
        public RealNameStatus realNameStatus = RealNameStatus.NotNeed;  // 实名制状态
        public bool isAdult = true;                                     // 是否成年
        public int onlineTime = 0;                                      // 今日在线时长
        public bool isNight = false;                                    // 是否是深夜

        public RealNameData(){}

        public RealNameData(bool canPlay, RealNameStatus realNameStatus, bool isAdult, int onlineTime, bool isNight)
        {
            this.realNameStatus = realNameStatus;
            this.onlineTime = onlineTime;
            this.canPlay = canPlay;
            this.isNight = isNight;
            this.isAdult = isAdult;
        }
    }
}