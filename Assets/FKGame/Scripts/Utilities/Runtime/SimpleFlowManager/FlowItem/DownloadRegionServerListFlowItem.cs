using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class DownloadRegionServerListFlowItem : FlowItemBase
    {
        private string[] regionServerURLs;
        private static Dictionary<string, ContinentCountryTableData> continentCountryTableDic = new Dictionary<string, ContinentCountryTableData>();
        public const string P_IPGeolocationDetail = "IPGeolocationDetail";
        public const string P_GameServerAreaData = "GameServerAreaData";
        public const string P_GameServerAreaDataConfigURL = "GameServerAreaDataConfigURL";
        public const string P_Country_Code = "country_Code";
        public IPGeolocationDetail iPGeolocationDetail;
        private static int retryTimes = 0;
        private int index = 0;
        public bool IsChinaIP;
        protected override void OnFlowStart(params object[] paras)
        {
            index = 0;
            IPGeolocationManager.GetIPGeolocation(ReciveIPDetail);
        }

        internal void SetURLs(string[] pathArr)
        {
            regionServerURLs = pathArr;
            if (regionServerURLs == null)
            {
                Debug.LogError("DownloadRegionServerListFlowItem.regionServerURLs is null!");
                return;
            }
        }
        
        private void ReciveIPDetail(IPGeolocationDetail detail)
        {
            if (detail == null)
            {
                retryTimes++;
                if (retryTimes > 1)
                {
                    retryTimes = 0;
                    RunDownloadRegionServer();
                }
                else
                {
                    OnFlowStart();
                }
                return;
            }
            iPGeolocationDetail = detail;
            if (detail.country_code == "CN")
                IsChinaIP = true;
            Debug.Log("IP������" + detail.ipv4 + " ����:" + detail.country);
            flowManager.SetVariable(P_IPGeolocationDetail, detail);
            RunDownloadRegionServer();
        }

        private void RunDownloadRegionServer()
        {
            if (index >= regionServerURLs.Length)
            {
                Finish("DownloadRegionServerList fail!");
                return;
            }
            string country_code = PlayerPrefs.GetString(P_Country_Code, "");
            if (string.IsNullOrEmpty(country_code))
                country_code = iPGeolocationDetail == null ? null : iPGeolocationDetail.country_code;
            Debug.Log("ʹ��Country code:" + country_code);
            string url = regionServerURLs[index];
            DownloadRegionServerList(url, country_code, (error, data) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError("RunDownloadRegionServer url:" + url + "\n error:" + error);
                    RunDownloadRegionServer();
                    return;
                }
                flowManager.SetVariable(P_GameServerAreaData, data);
                flowManager.SetVariable(P_GameServerAreaDataConfigURL, url);
                Finish(null);
            });
            index++;
        }

        // ���ش����������б�
        public void DownloadRegionServerList(string url, string country_code, Action<string, GameServerAreaData> OnCompleted)
        {
            DataTableExtend.DownLoadTableConfig<GameServerAreaData>(url, (dataList, urlError) =>
            {
                if (!string.IsNullOrEmpty(urlError))
                {
                    Debug.LogError("DownloadRegionServerList download fail!");
                    if (OnCompleted != null)
                    {
                        OnCompleted("download fail! " + urlError, null);
                    }
                    return;
                }
                if (dataList.Count == 0)
                {
                    Debug.LogError("DownloadRegionServerList GameServerAreaData is Empty!");
                    if (OnCompleted != null)
                    {
                        OnCompleted("GameServerAreaData is Empty!", null);
                    }
                    return;
                }
                if (!string.IsNullOrEmpty(country_code))
                {

                    //���ݹ���ѡ�����
                    foreach (var item in dataList)
                    {
                        if (ArrayContains(item.m_CountryCode, country_code))
                        {
                            Debug.Log("����ѡ������key��" + item.m_key);
                            if (OnCompleted != null)
                            {
                                OnCompleted(null, item);
                            }
                            return;
                        }
                    }
                }
                //���ݴ���ѡ�����
                string continentName = GetContinentByCountryCode(country_code);
                if (!string.IsNullOrEmpty(continentName))
                {
                    foreach (var item in dataList)
                    {
                        if (ArrayContains(item.m_ContinentName, continentName))
                        {
                            Debug.Log("���ݴ���ѡ������key��" + item.m_key);
                            if (OnCompleted != null)
                            {
                                OnCompleted(null, item);
                            }
                            return;
                        }
                    }
                }

                Debug.Log("ʹ��pingѡ�������" + dataList.Count);
                List<string> specialServerHostList = new List<string>();
                foreach (var item in dataList)
                {
                    specialServerHostList.Add(item.m_SpecialServerHost);
                }
                UnityPingManager.PingGetOptimalItem(specialServerHostList.ToArray(), (statistics) =>
                {
                    Debug.Log("ѡ������Ping:" + statistics);
                    GameServerAreaData saData = null;
                    foreach (var item in dataList)
                    {
                        if (item.m_SpecialServerHost == statistics.host)
                        {
                            saData = item;
                            break;
                        }
                    }
                    string error = null;
                    if (saData == null)
                    {
                        error = "Select Ping Result Error!";
                    }
                    if (OnCompleted != null)
                    {
                        OnCompleted(error, saData);
                    }
                });
            });
        }

        private static bool ArrayContains<T>(T[] arrays, T a)
        {
            if (arrays == null || arrays.Length == 0)
                return false;
            foreach (var item in arrays)
            {
                if (item.Equals(a))
                {
                    return true;
                }
            }
            return false;
        }

        /// ��ù��������ڴ��ޣ����ش�����д
        public static string GetContinentByCountryCode(string countryCode)
        {
            if (continentCountryTableDic.Count == 0)
            {
                try
                {
                    TextAsset textAsset = Resources.Load<TextAsset>("ContinentCountryTable");
                    ContinentCountryTableData[] data = JsonSerializer.FromJson<ContinentCountryTableData[]>(textAsset.text);
                    foreach (var item in data)
                    {
                        continentCountryTableDic.Add(item.country_code, item);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            if (continentCountryTableDic.ContainsKey(countryCode))
            {
                return continentCountryTableDic[countryCode].continent_name;
            }
            else
            {
                Debug.LogError("����ȱʧ��û���ҵ���Ӧ�Ĵ��� countryCode��" + countryCode);
                return null;
            }
        }
    }
}