using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public enum GoodsBuyTimeLimitType
    {
        Forever,        // ��Զ������������������ֻ�ܹ���1�Σ�
        TimeRange,      // ����ʱ�䷶Χ
        PerTime,        // ����ѭ��ʱ�� ÿ��ÿ��ô��ÿ�µȵ�
    }

    // ���ƹ���ʱ��
    public class GoodsBuyTimeLimitInfo
    {
        private const string TimeFormat = "yyyy-MM-dd HH:mm:ss";
        public GoodsBuyTimeLimitType timeLimitType = GoodsBuyTimeLimitType.Forever;
        public String timeRange;        // ʱ�䷶Χ����ʽ��2019-01-12 00:00:00=2019-02-01 12:00:00��,������Ϊnull
        public String timePerString;    // ֱ�ӷ��أ�ÿ�գ�ÿ��,ÿ�µĶ������ֶΣ�
        public string GetTimePerString()
        {
            return timePerString;
        }

        // ���ʱ�䷶Χ
        public void GetTimeRange(out DateTime startTime, out DateTime endTime)
        {
            String[] arrs = timeRange.Split('=');
            startTime = DateTime.Now;
            endTime = DateTime.Now;
            try
            {
                startTime = DateTime.ParseExact(arrs[0], TimeFormat, null);
                endTime = DateTime.ParseExact(arrs[1], TimeFormat, null);
            }
            catch (Exception e)
            {
                Debug.LogError("ת��ʱ���ʽʧ�ܣ�" + timeRange + "\n" + e);
            }
        }
    }
}