namespace FKGame
{
    // ��sdk ��ȡ����Ʒ��Ϣ
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