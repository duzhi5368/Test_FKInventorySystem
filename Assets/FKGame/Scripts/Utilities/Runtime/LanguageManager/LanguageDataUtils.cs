using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LanguageDataUtils
    {
        public static LanguageSettingConfig LoadRuntimeConfig()
        {
            if (ResourcesConfigManager.IsResourceExist(LanguageManager.c_configFileName))
            {
                LanguageSettingConfig config;
                string json = ResourceManager.LoadText(LanguageManager.c_configFileName);
                ResourceManager.DestoryAssetsCounter(LanguageManager.c_configFileName);
                if (!string.IsNullOrEmpty(json))
                    config = JsonSerializer.FromJson<LanguageSettingConfig>(json);
                else
                {
                    config = null;
                }
                return config;
            }
            else
            {
                return null;
            }
        }

        public static void SaveEditorConfig(LanguageSettingConfig config)
        {
            string json = JsonSerializer.ToJson(config);
            FileUtils.CreateTextFile(ResourcesMacro.LANGUAGE_DATA_SAVE_PATH_DIR + LanguageManager.c_configFileName + ".txt", json);
        }

        public static DataTable LoadFileData(SystemLanguage language, string fullKeyFileName)
        {
            if (string.IsNullOrEmpty(fullKeyFileName))
                return null;
            string path = GetLanguageSavePath(language, fullKeyFileName);
            string text = FileUtils.LoadTextFileByPath(path);
            return DataTable.Analysis(text);
        }

        public static string GetLanguageSavePath(SystemLanguage langeuageName, string fullkeyFileName)
        {
            return ResourcesMacro.LANGUAGE_DATA_SAVE_PATH_DIR + langeuageName + "/" + LanguageManager.GetLanguageDataName(langeuageName, fullkeyFileName) + ".txt";
        }
    }
}