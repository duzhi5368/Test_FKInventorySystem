namespace FKGame
{
    // 登陆错误枚举
    public enum LoginErrorEnum
    {
        None,
        GameCenterNotOpen,
        NoInstallApp,       // 没有安装相应的app
        SDKError            // SDK 传入的错误
    }

    // 定义支付商店名字
    public enum StoreName
    {
        None = 0,
        GooglePay = 1,
        AmazonAppStore = 2,
        AppleAppStore = 3,
        Windows = 4,
        FacebookStore = 5,
    }

    public enum GoodsType
    {
        NORMAL,             // 可以反复购买的商品
        ONCE_ONLY,          // 只能购买一次的商品
        RIGHTS,             // 购买持续一段时间的商品，例如会员
    }

    // 支付限制类型
    public enum PayLimitType
    {
        None,           // 无限制
        NoRealName,     // 未完成实名认证
        ChildLimit,     // 未成年，超出了限制
    }

    // SDK的功能类型
    public enum SDKType
    {
        Log,
        Login,
        AD,
        Pay,
        RealName,
        Other,
    }

    // 查看广告的结果
    public enum ADResult
    {
        Load_Success,
        Load_Failure,

        Show_Start,
        Show_Click,
        Show_Failed,
        Show_Skipped,
        Show_Finished
    }
}