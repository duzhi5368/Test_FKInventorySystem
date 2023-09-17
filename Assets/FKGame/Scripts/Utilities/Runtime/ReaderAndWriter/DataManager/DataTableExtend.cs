using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
//------------------------------------------------------------------------
namespace FKGame
{
    public class DataTableExtend
    {
        public static List<T> GetTableDatas<T>(string tableText) where T : IDataGenerateBase, new()
        {
            List<T> listData = new List<T>();
            try
            {
                DataTable data = DataTable.Analysis(tableText);
                for (int i = 0; i < data.tableIDDict.Count; i++)
                {
                    string key = data.tableIDDict[i];
                    T item = new T();
                    item.LoadData(data, key);
                    listData.Add(item);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("【FK】Parser dataTable error: " + e);
            }
            return listData;
        }

        // 从网络上下载配置并转换成表格
        public static void DownLoadTableConfig<T>(string url, Action<List<T>, string> callBack) where T : IDataGenerateBase, new()
        {
            try
            {
                MonoBehaviourRuntime.Instance.StartCoroutine(DownLoadText<T>(url, callBack));
            }
            catch (Exception e)
            {
                if (callBack != null)
                {
                    callBack(null, e.ToString());
                }
            }
        }

        static IEnumerator DownLoadText<T>(string url, Action<List<T>, string> callBack) where T : IDataGenerateBase, new()
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("【FK】Download data failed: URL is null");
                if (callBack != null)
                {
                    callBack(null, "URL is null");
                }
                yield break;
            }

#pragma warning disable CS0618
            WWW www = new WWW(url);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("【FK】Download data failed: URL = " + url + "\n error:" + www.error);
                if (callBack != null)
                {
                    callBack(null, www.error);
                }
            }
            else
            {
                List<T> configs = GetTableDatas<T>(www.text);
                if (callBack != null)
                {
                    callBack(configs, null);
                }
            }
#pragma warning restore CS0618
        }
    }
}