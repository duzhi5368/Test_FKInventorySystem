namespace FKGame
{
    public class RequestRealNameState2Client : IMessageClass
    {
        public int code;
        public long allPlayTime = 0;                                        // �����Ϸ��ʱ�� ���֣�
        public int onlineTime = 0;                                          // ��������ʱ��(��)
        public bool canPlay = true;                                         // �Ƿ���Լ�������
        public bool adult = true;                                           // �Ƿ����
        public bool night = false;                                          // �Ƿ�����ҹ��22ʱ������8ʱ ����Ϊδ�������ṩ��Ϸ����
        public RealNameStatus realNameStatus = RealNameStatus.NotNeed;      // ʵ����״̬

        public void DispatchMessage()
        {
            GlobalEvent.DispatchTypeEvent(this);
        }
    }
}