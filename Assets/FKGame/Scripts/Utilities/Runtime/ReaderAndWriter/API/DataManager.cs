using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ���ݹ�������ֻ�������ȸ��£���ʹ��Ĭ��ֵ��ͨ��ResourceManager����
    public class DataManager
    {
        public const string c_directoryName = "Data";
        public const string c_expandName = "txt";
        static Dictionary<string, DataTable> s_dataCache = new Dictionary<string, DataTable>(); // ���ݻ���

        public static bool GetIsExistData(string DataName)
        {
            return ResourcesConfigManager.GetIsExitRes(DataName);
        }

        public static DataTable GetData(string DataName)
        {
            try
            {
                // �༭���²�������
                if (s_dataCache.ContainsKey(DataName))
                {
                    return s_dataCache[DataName];
                }

                DataTable data = null;
                string dataJson = "";
                if (Application.isPlaying)
                {
                    dataJson = ResourceManager.LoadText(DataName);
                }
                else
                {
                    dataJson = ResourceIOTool.ReadStringByResource(PathTool.GetRelativelyPath(c_directoryName, DataName, c_expandName));
                }

                if (dataJson == "")
                {
                    throw new Exception("Dont Find ->" + DataName + "<-");
                }
                data = DataTable.Analysis(dataJson);
                data.m_tableName = DataName;

                s_dataCache.Add(DataName, data);
                return data;
            }
            catch (Exception e)
            {
                throw new Exception("GetData Exception ->" + DataName + "<- : " + e.ToString());
            }
        }

        // �������
        public static void CleanCache()
        {
            foreach (var item in s_dataCache.Keys)
            {
                ResourceManager.DestoryAssetsCounter(item);
            }
            s_dataCache.Clear();
        }
    }
}