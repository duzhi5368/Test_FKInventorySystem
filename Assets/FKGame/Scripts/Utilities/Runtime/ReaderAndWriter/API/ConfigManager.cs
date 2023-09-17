using System;
using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class ConfigManager
    {
        public const string directoryName = GlobeDefine.CONFIG_DIRECTORY;
        public const string expandName = "json";

        // ≈‰÷√ª∫¥Ê
        static Dictionary<string, Dictionary<string, SingleField>> configCache = new Dictionary<string, Dictionary<string, SingleField>>();

        public static bool IsConfigExist(string ConfigName)
        {
            return ResourcesConfigManager.IsResourceExist(ConfigName);
        }

        public static Dictionary<string, SingleField> GetData(string ConfigName)
        {
            if (configCache.ContainsKey(ConfigName))
            {
                return configCache[ConfigName];
            }

            string dataJson = "";
            dataJson = ResourceManager.LoadText(ConfigName);

            if (dataJson == "")
            {
                throw new Exception("°æFK°øConfigManager GetData not find " + ConfigName);
            }
            else
            {
                Dictionary<string, SingleField> config = JsonSerializer.Json2Dictionary<SingleField>(dataJson);

                configCache.Add(ConfigName, config);
                return config;
            }
        }

        public static SingleField GetData(string ConfigName, string key)
        {
            return GetData(ConfigName)[key];
        }

        public static void CleanCache()
        {
            foreach (var item in configCache.Keys)
            {
                ResourceManager.DestoryAssetsCounter(item);
            }
            configCache.Clear();
        }
    }
}
