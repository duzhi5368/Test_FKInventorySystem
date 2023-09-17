using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 数据管理器，只读，可热更新，可使用默认值。通过ResourceManager加载
    public class DataManager
    {
        public const string directoryName = GlobeDefine.DATA_DIRECTORY;
        public const string expandName = "txt";
        // 数据缓存
        static Dictionary<string, DataTable> dataCache = new Dictionary<string, DataTable>();

        public static bool GetIsExistData(string DataName)
        {
            return ResourcesConfigManager.IsResourceExist(DataName);
        }

        public static DataTable GetData(string DataName)
        {
            try
            {
                // 编辑器下不处理缓存
                if (dataCache.ContainsKey(DataName))
                {
                    return dataCache[DataName];
                }

                DataTable data = null;
                string dataJson = "";
                if (Application.isPlaying)
                {
                    dataJson = ResourceManager.LoadText(DataName);
                }
                else
                { 
                    dataJson = ResourceIOTool.ReadStringByResource(directoryName + "/" + DataName + "." + expandName);
                }

                if (dataJson == "")
                {
                    throw new Exception("【FK】Can't Find ->" + DataName + "<-");
                }
                data = DataTable.Analysis(dataJson);
                data.m_tableName = DataName;

                dataCache.Add(DataName, data);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception("【FK】GetData Exception ->" + DataName + "<- : " + e.ToString());
            }
        }

        // 清除缓存
        public static void CleanCache()
        {
            foreach (var item in dataCache.Keys)
            {
                ResourceManager.DestoryAssetsCounter(item);
            }
            dataCache.Clear();
        }
    }
}