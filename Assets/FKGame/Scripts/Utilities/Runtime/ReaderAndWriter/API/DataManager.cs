using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ���ݹ�������ֻ�������ȸ��£���ʹ��Ĭ��ֵ��ͨ��ResourceManager����
    public class DataManager
    {
        public const string directoryName = GlobeDefine.DATA_DIRECTORY;
        public const string expandName = "txt";
        // ���ݻ���
        static Dictionary<string, DataTable> dataCache = new Dictionary<string, DataTable>();

        public static bool GetIsExistData(string DataName)
        {
            return ResourcesConfigManager.IsResourceExist(DataName);
        }

        public static DataTable GetData(string DataName)
        {
            try
            {
                // �༭���²�������
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
                    throw new Exception("��FK��Can't Find ->" + DataName + "<-");
                }
                data = DataTable.Analysis(dataJson);
                data.m_tableName = DataName;

                dataCache.Add(DataName, data);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception("��FK��GetData Exception ->" + DataName + "<- : " + e.ToString());
            }
        }

        // �������
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