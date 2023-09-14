namespace FKGame
{
    // 从sdk 获取的商品信息
    public struct GoodsInfoFromSDK
    {
        public string goodsId;
        public string localizedPriceString;

        public GoodsInfoFromSDK(string goodsId, string localizedPriceString)
        {
            this.goodsId = goodsId;
            this.localizedPriceString = localizedPriceString;
        }
    }
}