namespace FKGame
{
    // �������������
    public class GeneralShopTableDataComplete2Client : IMessageClass
    {
        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}