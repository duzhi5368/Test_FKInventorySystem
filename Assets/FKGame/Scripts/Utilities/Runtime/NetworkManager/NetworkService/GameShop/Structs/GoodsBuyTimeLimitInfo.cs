using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public enum GoodsBuyTimeLimitType
    {
        Forever,        // 永远（限制终身，比如终身只能购买1次）
        TimeRange,      // 具体时间范围
        PerTime,        // 定义循环时间 每周每天么，每月等等
    }

    // 限制购买时间
    public class GoodsBuyTimeLimitInfo
    {
        private const string TimeFormat = "yyyy-MM-dd HH:mm:ss";
        public GoodsBuyTimeLimitType timeLimitType = GoodsBuyTimeLimitType.Forever;
        public String timeRange;        // 时间范围（格式：2019-01-12 00:00:00=2019-02-01 12:00:00）,不限制为null
        public String timePerString;    // 直接返回（每日，每周,每月的多语言字段）
        public string GetTimePerString()
        {
            return timePerString;
        }

        // 获得时间范围
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
                Debug.LogError("转换时间格式失败：" + timeRange + "\n" + e);
            }
        }
    }
}