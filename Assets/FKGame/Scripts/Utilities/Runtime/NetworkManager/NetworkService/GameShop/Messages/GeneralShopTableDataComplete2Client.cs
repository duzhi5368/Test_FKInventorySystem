namespace FKGame
{
    // 传输表格数据完成
    public class GeneralShopTableDataComplete2Client : IMessageClass
    {
        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}