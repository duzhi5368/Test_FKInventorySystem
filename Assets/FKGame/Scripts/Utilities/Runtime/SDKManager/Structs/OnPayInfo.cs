namespace FKGame
{
    public struct OnPayInfo
    {
        //����
        public bool isSuccess;
        public string error;        // ������Ϣ
        public string goodsId;
        public string orderID;
        public StoreName storeName;
        public string goodsName;
        public float price;
        public string currency;     // ��������
        public string receipt;      // ֧����ִ

        //ѡ��
        public GoodsType goodsType;
        public string userID;       // ���ID���������ʹ�õ�ID��

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