using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class GoodsDiscountInfo
    {
        public bool isDiscount = false;
        public float discountPercentage = 1;    // �۸���۱�����0-1��
        public String timeRange;                // ����ʱ�����ƣ���ʽ��2019-01-12 00:00:00=2019-02-01 12:00:00��,������Ϊnull

        public bool getDiscount()
        {
            return isDiscount;
        }
        public void setDiscount(bool isDiscount)
        {
            this.isDiscount = isDiscount;
        }
        public float getDiscountPercentage()
        {
            return discountPercentage;
        }
        public void setDiscountPercentage(float discountPercentage)
        {
            this.discountPercentage = discountPercentage;
        }
        public String getTimeRange()
        {
            return timeRange;
        }
        public void setTimeRange(String timeRange)
        {
            this.timeRange = timeRange;
        }
    }
}