namespace FKGame
{
    // Ҫ���û�ʹ�öһ���
    public class AskUserUseActivationCode2Client : IMessageClass
    {
        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}