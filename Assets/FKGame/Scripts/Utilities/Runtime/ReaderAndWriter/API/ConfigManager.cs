using System;
using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class ConfigManager
    {
        public const string c_directoryName = "Config";
        public const string c_expandName = "json";

        // ≈‰÷√ª∫¥Ê
        static Dictionary<string, Dictionary<string, SingleField>> s_configCache = new Dictionary<string, Dictionary<string, SingleField>>();

        public static bool GetIsExistConfig(string ConfigName)
        {
            return ResourcesConfigManager.GetIsExitRes(ConfigName);
        }

        public static Dictionary<string, SingleField> GetData(string ConfigName)
        {
            if (s_configCache.ContainsKey(ConfigName))
            {
                return s_configCache[ConfigName];
            }

            string dataJson = "";
            dataJson = ResourceManager.LoadText(ConfigName);

            if (dataJson == "")
            {
                throw new Exception("ConfigManager GetData not find " + ConfigName);
            }
            else
            {
                Dictionary<string, SingleField> config = JsonSerializer.Json2Dictionary<SingleField>(dataJson);

                s_configCache.Add(ConfigName, config);
                return config;
            }
        }

        public static SingleField GetData(string ConfigName, string key)
        {
            return GetData(ConfigName)[key];
        }

        public static void CleanCache()
        {
            foreach (var item in s_configCache.Keys)
            {
                ResourceManager.DestoryAssetsCounter(item);
            }
            s_configCache.Clear();
        }
    }
}
