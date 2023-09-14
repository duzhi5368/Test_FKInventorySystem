namespace FKGame
{
    public class RequestRealNameState2Client : IMessageClass
    {
        public int code;
        public long allPlayTime = 0;                                        // 玩家游戏总时长 （分）
        public int onlineTime = 0;                                          // 今日在线时长(秒)
        public bool canPlay = true;                                         // 是否可以继续游玩
        public bool adult = true;                                           // 是否成年
        public bool night = false;                                          // 是否是深夜（22时至次日8时 不得为未成年人提供游戏服务）
        public RealNameStatus realNameStatus = RealNameStatus.NotNeed;      // 实名制状态

        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}