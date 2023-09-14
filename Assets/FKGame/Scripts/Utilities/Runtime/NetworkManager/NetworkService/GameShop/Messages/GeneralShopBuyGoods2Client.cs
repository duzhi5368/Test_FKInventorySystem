using System;
//------------------------------------------------------------------------
namespace FKGame
{
    // 购买返回消息
    public class GeneralShopBuyGoods2Client : CodeMessageBase
    {
        public String shopType;     // 商店Type
        public String goodsID;      // 物品ID
        public int buyNum;          // 购买数量

        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}