namespace FKGame
{
    /// <summary>
    /// ���ػ���Ʒ��Ϣ������SDKƽ̨�ĺ�̨���磺
    /// localizedPriceString :$6.00
    /// localizedTitle :С����ʯ
    /// localizedDescription :��ʯ��������Ϸ�����ڹ��������Ʒ
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
            this.isoCurrencyCode = isoCurrencyCode;     // Ĭ�������
            this.localizedPriceString = price.ToString();
        }
        public string goodsID;              // id
        public string localizedPriceString; // ������
        public string localizedTitle;       // ����
        public string localizedDescription; // �̵�����
        public string isoCurrencyCode;      // ��������
        public float localizedPrice;        // �۸�
        public GoodsType goodsType;         // ��Ʒ����
    }
}