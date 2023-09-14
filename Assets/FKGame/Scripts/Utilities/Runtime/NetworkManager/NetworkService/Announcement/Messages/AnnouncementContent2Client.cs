using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AnnouncementContent2Client : IMessageClass
    {
        public string id;
        public string titleName;
        public string content;
        public List<GameRewardData> rewardDatas = new List<GameRewardData>();
        public string useTag;       // 用来分辨消息用途是公告，还是排名奖励等
        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}