using System.Collections.Generic;
using System.IO;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class RecordManager
    {
        public const string directoryName = GlobeDefine.SAVE_RECORD_DIRECTORY;
        public const string expandName = "json";
        // ¼ÇÂ¼»º´æ
        static Dictionary<string, RecordTable> s_RecordCache = new Dictionary<string, RecordTable>();
        static Deserializer des = new Deserializer();

        public static RecordTable GetData(string RecordName)
        {
            if (s_RecordCache.ContainsKey(RecordName))
            {
                return s_RecordCache[RecordName];
            }
            RecordTable record = null;
            string dataJson = "";
            string fullPath = PathTool.GetAbsolutePath(ResLoadLocation.Persistent, PathTool.GetRelativelyPath(directoryName,RecordName,expandName));
            if (File.Exists(fullPath))
            {
                dataJson = ResourceIOTool.ReadStringByFile(fullPath);   // ¼ÇÂ¼ÓÀÔ¶´ÓÉ³ºÐÂ·¾¶¶ÁÈ¡
            }
            if (dataJson == "")
            {
                record = new RecordTable();
            }
            else
            {
                record = RecordTable.Analysis(dataJson);
            }
            s_RecordCache.Add(RecordName, record);
            return record;
        }

        public static void SaveData(string RecordName, RecordTable data)
        {
#if !UNITY_WEBGL
            ResourceIOTool.WriteStringByFile(PathTool.GetAbsolutePath(ResLoadLocation.Persistent,PathTool.GetRelativelyPath(directoryName,RecordName,expandName)),RecordTable.Serialize(data));
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.AssetDatabase.Refresh();
            }
#endif
#endif
        }

        public static void CleanRecord(string recordName)
        {
            RecordTable table = GetData(recordName);
            table.Clear();
            SaveData(recordName, table);
        }

        public static void CleanAllRecord()
        {
            FileTool.DeleteDirectory(Application.persistentDataPath + "/" + RecordManager.directoryName);
            CleanCache();
        }

        public static void CleanCache()
        {
            s_RecordCache.Clear();
        }

        public static void SaveRecord(string RecordName, string key, string value)
        {
            RecordTable table = GetData(RecordName);
            table.SetRecord(key, value);
            SaveData(RecordName, table);
        }

        public static void SaveRecord(string RecordName, string key, int value)
        {
            RecordTable table = GetData(RecordName);
            table.SetRecord(key, value);
            SaveData(RecordName, table);
        }

        public static void SaveRecord(string RecordName, string key, bool value)
        {
            RecordTable table = GetData(RecordName);
            table.SetRecord(key, value);
            SaveData(RecordName, table);
        }

        public static void SaveRecord(string RecordName, string key, float value)
        {
            RecordTable table = GetData(RecordName);
            table.SetRecord(key, value);
            SaveData(RecordName, table);
        }

        public static void SaveRecord(string RecordName, string key, Vector2 value)
        {
            RecordTable table = GetData(RecordName);
            table.SetRecord(key, value);
            SaveData(RecordName, table);
        }

        public static void SaveRecord(string RecordName, string key, Vector3 value)
        {
            RecordTable table = GetData(RecordName);
            table.SetRecord(key, value);
            SaveData(RecordName, table);
        }

        public static void SaveRecord(string RecordName, string key, Color value)
        {
            RecordTable table = GetData(RecordName);
            table.SetRecord(key, value);
            SaveData(RecordName, table);
        }

        public static void SaveRecord<T>(string RecordName, string key, T value)
        {
            string content = Serializer.Serialize(value);
            SaveRecord(RecordName, key, content);
        }

        public static int GetIntRecord(string RecordName, string key, int defaultValue)
        {
            RecordTable table = GetData(RecordName);
            return table.GetRecord(key, defaultValue);
        }

        public static string GetStringRecord(string RecordName, string key, string defaultValue)
        {
            RecordTable table = GetData(RecordName);
            return table.GetRecord(key, defaultValue);
        }

        public static bool GetBoolRecord(string RecordName, string key, bool defaultValue)
        {
            RecordTable table = GetData(RecordName);
            return table.GetRecord(key, defaultValue);
        }

        public static float GetFloatRecord(string RecordName, string key, float defaultValue)
        {
            RecordTable table = GetData(RecordName);
            return table.GetRecord(key, defaultValue);
        }

        public static Vector2 GetVector2Record(string RecordName, string key, Vector2 defaultValue)
        {
            RecordTable table = GetData(RecordName);
            return table.GetRecord(key, defaultValue);
        }

        public static Vector3 GetVector3Record(string RecordName, string key, Vector3 defaultValue)
        {
            RecordTable table = GetData(RecordName);
            return table.GetRecord(key, defaultValue);
        }

        public static Color GetColorRecord(string RecordName, string key, Color defaultValue)
        {
            RecordTable table = GetData(RecordName);
            return table.GetRecord(key, defaultValue);
        }

        public static T GetTRecord<T>(string RecordName, string key, T defaultValue)
        {
            string content = GetStringRecord(RecordName, key, null);
            if (content == null)
            {
                return defaultValue;
            }
            else
            {
                return des.Deserialize<T>(content);
            }
        }
    }
}