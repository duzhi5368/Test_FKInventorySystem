namespace FKGame
{
    // ˢ�µ��̵���Ϣ
    public class UpdateGeneralShopInfo2Client : IMessageClass
    {
        public GameShopInfoData shopInfo = null;        // ˢ�µ��̵���Ϣ

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