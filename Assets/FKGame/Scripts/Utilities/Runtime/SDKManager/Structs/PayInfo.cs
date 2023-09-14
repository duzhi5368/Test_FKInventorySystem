namespace FKGame
{
    public struct PayInfo
    {
        public string userID;
        public string goodsID;
        public string goodsName;
        public string storeName;
        public string prepay_id;
        public string tag;
        public GoodsType goodsType;
        public string orderID;
        public float price;
        public string currency;   //ªı±“¿‡–Õ

        public PayInfo(string goodsID, string goodsName, string tag, GoodsType goodsType, 
            string orderID, float price, string currency, string userID, string storeName)
        {
            this.userID = userID;
            this.goodsID = goodsID;
            this.goodsName = goodsName;
            this.tag = tag;
            this.goodsType = goodsType;
            this.orderID = orderID;
            this.price = price;
            this.currency = currency;
            this.storeName = storeName;
            prepay_id = null;
        }
    }
}