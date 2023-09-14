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
        public string useTag;       // �����ֱ���Ϣ��;�ǹ��棬��������������
        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}