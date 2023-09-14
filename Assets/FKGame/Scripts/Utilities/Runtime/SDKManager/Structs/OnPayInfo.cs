namespace FKGame
{
    public struct OnPayInfo
    {
        //必填
        public bool isSuccess;
        public string error;        // 错误信息
        public string goodsId;
        public string orderID;
        public StoreName storeName;
        public string goodsName;
        public float price;
        public string currency;     // 货币类型
        public string receipt;      // 支付回执

        //选填
        public GoodsType goodsType;
        public string userID;       // 玩家ID（后端生成使用的ID）

        public OnPayInfo(PayInfo payInfo, bool isSuccess, StoreName storeName)
        {
            this.isSuccess = isSuccess;
            this.goodsId = payInfo.goodsID;
            this.orderID = null;
            this.storeName = storeName;
            this.goodsName = payInfo.goodsName;
            this.price = payInfo.price;
            this.currency = null;
            this.receipt = null;
            this.goodsType = payInfo.goodsType;
            userID = null;
            error = null;
        }
    }
}