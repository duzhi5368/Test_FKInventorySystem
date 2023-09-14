namespace FKGame
{
    // 刷新的商店信息
    public class UpdateGeneralShopInfo2Client : IMessageClass
    {
        public GameShopInfoData shopInfo = null;        // 刷新的商店信息

        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }

        public GameShopInfoData getShopInfo()
        {
            return shopInfo;
        }

        public void setShopInfo(GameShopInfoData shopInfo)
        {
            this.shopInfo = shopInfo;
        }
    }
}