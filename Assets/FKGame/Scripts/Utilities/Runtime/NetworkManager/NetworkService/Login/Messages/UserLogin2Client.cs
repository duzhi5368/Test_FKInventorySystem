namespace FKGame
{
    public class UserLogin2Client : CodeMessageBase
    {
        public User user;
        public bool newPlayerState;                     // �Ƿ�������ҵ�¼
        public bool reloginState = false;               // ����Ƿ�������
        public bool supportCompressMsg = false;         // �������Ƿ�֧����Ϣѹ��

        public override void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}