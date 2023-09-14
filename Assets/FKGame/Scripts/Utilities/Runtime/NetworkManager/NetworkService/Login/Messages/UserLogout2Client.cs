namespace FKGame
{
    public class UserLogout2Client : IMessageClass
    {
        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}