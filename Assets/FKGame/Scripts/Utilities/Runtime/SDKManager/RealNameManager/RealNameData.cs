namespace FKGame
{
    // ʵ������Ϣ
    public class RealNameData
    {
        public bool canPlay = true;                                     // �Ƿ���Լ�������
        public RealNameStatus realNameStatus = RealNameStatus.NotNeed;  // ʵ����״̬
        public bool isAdult = true;                                     // �Ƿ����
        public int onlineTime = 0;                                      // ��������ʱ��
        public bool isNight = false;                                    // �Ƿ�����ҹ

        public RealNameData(){}

        public RealNameData(bool canPlay, RealNameStatus realNameStatus, bool isAdult, int onlineTime, bool isNight)
        {
            this.realNameStatus = realNameStatus;
            this.onlineTime = onlineTime;
            this.canPlay = canPlay;
            this.isNight = isNight;
            this.isAdult = isAdult;
        }
    }
}