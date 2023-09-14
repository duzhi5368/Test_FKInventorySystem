using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 实名制检测的结果
    public class RealNameLimitEvent
    {
        public RealNameLimitType realNameLimitType;     // 限制类型
        public RealNameData realNameData;               // 实名制信息
        public string describ = "";                     // 描述

        public RealNameLimitEvent(RealNameData l_realNameData, string l_describ = "")
        {
            realNameData = l_realNameData;
            describ = l_describ;
            realNameLimitType = GetLimitResult(realNameData);
        }

        private RealNameLimitType GetLimitResult(RealNameData l_realNameData)
        {
            if (!l_realNameData.canPlay) // 禁止继续游玩
            {
                if (l_realNameData.realNameStatus == RealNameStatus.NotRealName)
                {
                    if (string.IsNullOrEmpty(describ))
                    {
                        describ = "根据相关法律法规，未实名的玩家每天只能体验1个小时哦，请前往实名认证吧";
                    }
                    // 未实名制，游戏体验上限1小时
                    return RealNameLimitType.NoRealNameMaxTimeLimit;
                }
                else if (!l_realNameData.isAdult)
                {
                    if (l_realNameData.isNight)
                    {
                        if (string.IsNullOrEmpty(describ))
                        {
                            describ = "晚上10点过了就要休息咯，请明天早上8点之后再来吧";
                        }
                        // 深夜， 22时至次日8时 不得为未成年人提供游戏服务
                        return RealNameLimitType.ChildNightLimit;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(describ))
                        {
                            describ = "您今天的游玩时间已经超过了1.5小时哦（节假日3小时），请明天再来吧";
                        }
                        // 未成年人，每日在线时长不得超过x小时（法定节假日3小时，其他日期1.5小时）
                        return RealNameLimitType.ChildTimeLimit;
                    }
                }
                else
                {
                    Debug.LogError("GetLimitResult error： adult:" + l_realNameData.isAdult);
                    return RealNameLimitType.NoLimit;
                }
            }
            else
            {
                return RealNameLimitType.NoLimit;// 可以玩，表示未受限制
            }
        }

        static public void Dispatch(int l_onlineTime, bool l_isNight, bool l_canPlay, RealNameStatus l_realNameStatus, bool l_isAdult)
        {
            RealNameData realNameData = new RealNameData(l_canPlay, l_realNameStatus, l_isAdult, l_onlineTime, l_isNight);
            GlobalEvent.DispatchTypeEvent(new RealNameLimitEvent(realNameData));
        }
    }
}