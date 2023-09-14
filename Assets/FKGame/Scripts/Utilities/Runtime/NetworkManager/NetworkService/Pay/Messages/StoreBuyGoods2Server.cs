namespace FKGame
{
    public class StoreBuyGoods2Server
    {
        public string id;           // 物品id
        public int number;          // 数量
        public StoreName storeName;
        public string receipt;

        public StoreBuyGoods2Server()
        {
        }

        public StoreBuyGoods2Server(string id, int number, StoreName storeName, string receipt)
        {
            this.id = id;
            this.number = number;
            this.storeName = storeName;
            this.receipt = receipt;
        }

        public static void SenBuyMsg(string id, int number, StoreName storeName, string receipt)
        {
            JsonMessageProcessingController.SendMessage(new StoreBuyGoods2Server(id, number, storeName, receipt));
        }
    }
}