namespace FKGame
{
    // �����������
    public class GoodsBuyNumOfTimeLimitInfo
    {
        public int buyTimesLimit = -1;      // ����������ƣ�-1ʱ��ʾ�����ƣ�
        public int alreadyBuyTimes = 0;     // ��ǰ���Ʒ�Χ���ѹ������,��������(����ÿ�ܹ���3�Σ����ܽ�������Ŀ)
        public int totalBuyTimes = 0;       // �ܹ������������������)

        public int getBuyTimesLimit()
        {
            return buyTimesLimit;
        }
        public void setBuyTimesLimit(int buyTimesLimit)
        {
            this.buyTimesLimit = buyTimesLimit;
        }
        public int getAlreadyBuyTimes()
        {
            return alreadyBuyTimes;
        }
        public void setAlreadyBuyTimes(int alreadyBuyTimes)
        {
            this.alreadyBuyTimes = alreadyBuyTimes;
        }
        public int getTotalBuyTimes()
        {
            return totalBuyTimes;
        }
        public void setTotalBuyTimes(int totalBuyTimes)
        {
            this.totalBuyTimes = totalBuyTimes;
        }
    }
}