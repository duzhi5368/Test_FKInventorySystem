using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UnityPingManager
    {
        private static bool isRunPingGetOptimalItem = false;
        private static List<UnityPingStatistics> pingStatistics = new List<UnityPingStatistics>();

        public static void Ping(string host, float timeOut = 4f, int pingTimes = 4, Action<string, UnityPingStatistics> resultCallBack = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Start Ping " + host + "\n");
            UnityPing.CreatePing(host, (res) =>
            {
                string stateInfo = res.isSuccess ? "Success" : res.errorReason.ToString();
                builder.Append("return:" + stateInfo + " time:" + res.pingTime + "\n");

            }, OnComplete: (pingResults) =>
            {
                int passTimes = 0;
                int maxTime = 0;
                int minTime = -1;
                int allTime = 0;
                int caculateTimes = 0;

                string ip = "";
                foreach (var item in pingResults)
                {
                    ip = item.ip;
                    if (item.isSuccess)
                    {
                        passTimes++;
                        if (maxTime < item.pingTime)
                        {
                            maxTime = item.pingTime;
                        }
                        if (minTime == -1)
                            minTime = item.pingTime;
                        if (minTime > item.pingTime)
                        {
                            minTime = item.pingTime;
                        }
                        allTime += item.pingTime;
                        caculateTimes++;
                    }
                }
                int averagePingTime = caculateTimes == 0 ? 0 : (allTime / caculateTimes);
                UnityPingStatistics statistics = new UnityPingStatistics(host, ip, pingResults.Length, passTimes, maxTime, minTime, averagePingTime);
                builder.Append("Ping Times:" + statistics.pingTimes + " Pass Times:" + statistics.pingPass + "\n");
                if (passTimes > 0)
                {
                    builder.Append("Min Ping:" + statistics.minPingTime + "ms Max Ping:" + statistics.maxPingTime + "ms AVG:" + statistics.averagePingTime + "\n");
                }
                if (resultCallBack != null)
                {
                    resultCallBack(builder.ToString(), statistics);
                }
                else
                {
                    Debug.Log(builder);
                }
            }, timeOut: timeOut, runTimes: pingTimes);
        }

        // 选取ping中最好的返回
        public static void PingGetOptimalItem(string[] hostList, Action<UnityPingStatistics> resultCallBack, float timeOut = 4f, int pingTimes = 4)
        {
            if (isRunPingGetOptimalItem)
                return;
            isRunPingGetOptimalItem = true;
            pingStatistics.Clear();

            if (hostList == null || hostList.Length == 0)
            {
                Debug.LogError("hostList is Empty!");
                isRunPingGetOptimalItem = false;
                SelectPingResult(pingStatistics, resultCallBack);
                return;
            }
            foreach (var host in hostList)
            {
                Ping(host, timeOut, pingTimes, (str, statistics) =>
                {
                    pingStatistics.Add(statistics);
                    if (pingStatistics.Count >= hostList.Length)
                    {
                        isRunPingGetOptimalItem = false;
                        SelectPingResult(pingStatistics, resultCallBack);
                    }
                });
            }
        }

        private static void SelectPingResult(List<UnityPingStatistics> pingStatistics, Action<UnityPingStatistics> resultCallBack)
        {
            if (pingStatistics.Count <= 1)
            {
                if (pingStatistics.Count == 0)
                {
                    if (resultCallBack != null)
                    {
                        resultCallBack(default(UnityPingStatistics));
                    }
                }
                else
                {
                    if (resultCallBack != null)
                    {
                        resultCallBack(pingStatistics[0]);
                    }
                }
            }
            else
            {
                List<UnityPingStatistics> tempStatistics = new List<UnityPingStatistics>();
                // 选取ping通率在50%及以上的
                foreach (var ss in pingStatistics)
                {
                    if (ss.pingPass * 1f / ss.pingTimes >= 0.5f)
                    {
                        tempStatistics.Add(ss);
                    }
                }

                if (tempStatistics.Count == 0)
                {
                    tempStatistics.AddRange(pingStatistics);
                }
                // 选取平均Ping最少的
                UnityPingStatistics minPing = tempStatistics[0];
                foreach (var ss in tempStatistics)
                {
                    if (ss.averagePingTime < minPing.averagePingTime && ss.pingPass > 0)
                    {
                        minPing = ss;
                    }
                }
                if (resultCallBack != null)
                {
                    resultCallBack(minPing);
                }
            }
        }
    }
}