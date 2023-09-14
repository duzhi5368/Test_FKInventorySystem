namespace FKGame
{
    /// <summary>
    /// 本地化商品信息，来自SDK平台的后台，如：
    /// localizedPriceString :$6.00
    /// localizedTitle :小包钻石
    /// localizedDescription :钻石可以在游戏内用于购买各项商品
    /// isoCurrencyCode :CNY
    /// localizedPrice :6
    /// </summary>
    public class LocalizedGoodsInfo
    {
        public LocalizedGoodsInfo()
        {
        }
        public LocalizedGoodsInfo(string goodsID, GoodsType goodsType, float price, string isoCurrencyCode = "CNY", string goodName = "")
        {
            this.goodsID = goodsID;
            this.goodsType = goodsType;
            this.localizedPrice = price;
            this.localizedTitle = goodName;
            this.isoCurrencyCode = isoCurrencyCode;     // 默认人民币
            this.localizedPriceString = price.ToString();
        }
        public string goodsID;              // id
        public string localizedPriceString; // 描述串
        public string localizedTitle;       // 标题
        public string localizedDescription; // 商店描述
        public string isoCurrencyCode;      // 货币类型
        public float localizedPrice;        // 价格
        public GoodsType goodsType;         // 商品类型
    }
}