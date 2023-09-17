using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class URLManager
    {
        const string s_configName = "URLConfig";
        static Dictionary<string, SingleField> s_URLTable;

        public static string GetURL(string urlKey)
        {
            Init();
            if (s_URLTable != null)
            {
                if (s_URLTable.ContainsKey(urlKey))
                {
                    return s_URLTable[urlKey].GetString();
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public static void Init()
        {
            if (s_URLTable == null)
            {
                if (ConfigManager.IsConfigExist(s_configName))
                {
                    s_URLTable = ConfigManager.GetData(s_configName);
                }
            }
        }
    }
}