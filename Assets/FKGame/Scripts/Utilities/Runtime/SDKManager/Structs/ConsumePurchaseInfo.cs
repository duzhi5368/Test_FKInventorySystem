namespace FKGame
{
    // ������Ʒ��Ϣ
    public struct ConsumePurchaseInfo
    {
        public string goodsId;
        public string token;

        public ConsumePurchaseInfo(string goodsId, string token)
        {
            this.goodsId = goodsId;
            this.token = token;
        }
    }
}