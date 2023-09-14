namespace FKGame
{
    public class StoreBuyGoods2Client : IMessageClass
    {
        public int code;
        public string id;
        public int number;
        public bool repeatReceipt = false;      // 是否是重复的凭据
        public string receipt;

        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}