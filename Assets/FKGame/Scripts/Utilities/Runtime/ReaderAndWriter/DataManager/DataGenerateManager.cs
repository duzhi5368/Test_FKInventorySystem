using System.Collections.Generic;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class DataGenerateManager<T> where T : IDataGenerateBase, new()
    {
        static Dictionary<string, T> s_dict = new Dictionary<string, T>();
        static List<T> s_ListData = new List<T>();
        static bool s_isInit = false;
        static string s_dataName = null;

        public static string DataName
        {
            get
            {
                if (s_dataName == null)
                {
                    s_dataName = typeof(T).Name.Replace("Generate", "");
                }
                return s_dataName;
            }
        }

        public static T GetData(string key)
        {
            if (key == null)
            {
                throw new Exception("【FK】DataGenerateManager<" + typeof(T).Name + "> GetData key is Null !");
            }

            // 清理缓存
            if (!s_isInit)
            {
                s_isInit = true;
                GlobalEvent.AddEvent(MemoryEvent.FreeHeapMemory, CleanCache);
            }

            if (s_dict.ContainsKey(key))
            {
                return s_dict[key];
            }
            else
            {
                T data = new T();
                data.LoadData(key);
                s_dict.Add(key, data);
                s_ListData.Add(data);
                return data;
            }
        }

        public static bool GetExistKey(string key)
        {
            return DataManager.GetData(DataName).ContainsKey(key);
        }

        // 全查表
        public static void PreLoad()
        {
            //清理缓存
            if (!s_isInit)
            {
                s_isInit = true;
                GlobalEvent.AddEvent(MemoryEvent.FreeHeapMemory, CleanCache);
            }

            DataTable data = GetDataTable();
            for (int i = 0; i < data.tableIDDict.Count; i++)
            {
                GetData(data.tableIDDict[i]);
            }
        }

        public static Dictionary<string, T> GetAllData()
        {
            CleanCache();
            PreLoad();
            return s_dict;
        }

        public static List<T> GetAllDataList()
        {
            CleanCache();
            PreLoad();
            return s_ListData;
        }

        public static DataTable GetDataTable()
        {
            return DataManager.GetData(DataName);
        }

        public static void CleanCache(params object[] objs)
        {
            s_dict.Clear();
            s_ListData.Clear();
        }
    }
}