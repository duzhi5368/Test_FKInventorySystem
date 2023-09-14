namespace FKGame
{
    public class GameRewardData
    {
        public string rewardType;       // 分类奖励的类型（如：金币，钻石，卡包）
        public string key;              // 用于二级分类或具体的东西（如卡牌类型的挥砍卡牌），不需要则不填写
        public int number;              // 数量
        public string reason;           // 发这个奖励的理由（选填）
    }
}