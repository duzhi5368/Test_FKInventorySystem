using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GeneralShopBuyGoods2Server
    {
        public String shopType;     // �̵�Type
        public String goodsID;      // ��ƷID
        public int buyNum;          // ��������

        public String getGoodsID()
        {
            return goodsID;
        }

        public void setGoodsID(String goodsID)
        {
            this.goodsID = goodsID;
        }

        public int getBuyNum()
        {
            return buyNum;
        }

        public void setBuyNum(int buyNum)
        {
            this.buyNum = buyNum;
        }

        public String getShopType()
        {
            return shopType;
        }

        public void setShopType(String shopType)
        {
            this.shopType = shopType;
        }
    }
}