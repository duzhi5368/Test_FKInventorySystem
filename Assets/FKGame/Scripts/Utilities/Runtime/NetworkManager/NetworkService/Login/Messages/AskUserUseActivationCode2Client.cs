namespace FKGame
{
    // 要求用户使用兑换码
    public class AskUserUseActivationCode2Client : IMessageClass
    {
        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}