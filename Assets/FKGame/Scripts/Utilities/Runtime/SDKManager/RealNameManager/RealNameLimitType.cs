namespace FKGame
{
    // 实名制检测结果
    public enum RealNameLimitType
    {
        NoLimit,                // 无限制
        NoRealNameMaxTimeLimit, // 未实名制，达到最大时间限制
        ChildNightLimit,        // 未成年，深夜限制（22时至次日8时 不得为未成年人提供游戏服务）
        ChildTimeLimit,         // 未成年，每日游戏时长限制
    }
}