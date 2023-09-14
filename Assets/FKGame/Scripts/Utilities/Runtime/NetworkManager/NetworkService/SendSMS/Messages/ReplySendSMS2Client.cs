namespace FKGame
{
    public class ReplySendSMS2Client : CodeMessageBase
    {
        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}