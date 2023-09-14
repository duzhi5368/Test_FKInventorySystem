using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class RemeedCodeUse2Client : CodeMessageBase
    {
        public List<GameRewardData> rewardDatas = new List<GameRewardData>();
        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}