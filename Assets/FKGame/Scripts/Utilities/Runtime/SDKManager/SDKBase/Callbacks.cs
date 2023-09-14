namespace FKGame
{
    public delegate void LoginCallBack(OnLoginInfo info);
    public delegate void PayCallBack(OnPayInfo info);
    public delegate void ADCallBack(OnADInfo info);
    public delegate void OtherCallBack(OnOtherInfo info);
    public delegate void GoodsInfoCallBack(GoodsInfoFromSDK info);
    public delegate void ConsumePurchaseCallBack(ConsumePurchaseInfo info);
    public delegate void RealNameCallBack(RealNameData info);
    public delegate void PayLimitCallBack(bool isLimit, int payAmount);
    public delegate void RealNameLogoutCallBack();
}