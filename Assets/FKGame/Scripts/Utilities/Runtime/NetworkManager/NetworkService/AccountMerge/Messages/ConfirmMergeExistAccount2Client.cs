namespace FKGame
{
    public class ConfirmMergeExistAccount2Client : CodeMessageBase
    {
        public LoginPlatform loginType;

        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}