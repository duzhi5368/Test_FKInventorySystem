using FKGame.Macro;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class LanguageDataEditorUtils
    {
        public static void SaveData(SystemLanguage langeuageName, string fullkeyFileName, DataTable data)
        {
            if (data == null)
                return;
            string path = ResourcesMacro.LANGUAGE_DATA_SAVE_PATH_DIR + langeuageName + "/" + LanguageManager.GetLanguageDataName(langeuageName, fullkeyFileName) + ".txt";
            string text = DataTable.Serialize(data);
            FileUtils.CreateTextFile(path, text);
        }

        // 加载所有某种语言的所有多语言文件，并转换成带"/"路径的数据
        public static List<string> LoadLangusgeAllFileNames(SystemLanguage language)
        {
            List<string> datas = new List<string>();

            string pathDir = ResourcesMacro.LANGUAGE_DATA_SAVE_PATH_DIR + language;
            if (Directory.Exists(pathDir))
            {
                string[] fileNames = PathUtils.GetDirectoryFileNames(pathDir, new string[] { ".txt" });
                foreach (var item in fileNames)
                {
                    string temp = item.Replace(LanguageManager.c_DataFilePrefix + language + "_", "").Replace("_", "/");
                    if (string.IsNullOrEmpty(temp))
                        continue;
                    datas.Add(temp);
                }
            }
            return datas;
        }

        public static List<string> GetLanguageAllFunKeyList()
        {
            List<string> list = new List<string>();
            LanguageSettingConfig config = LanguageDataUtils.LoadRuntimeConfig();
            if (config != null)
            {
                List<string> allFilePath = LoadLangusgeAllFileNames(config.defaultLanguage);
                foreach (var item in allFilePath)
                {
                    DataTable data = LanguageDataUtils.LoadFileData(config.defaultLanguage, item);
                    foreach (var key in data.tableIDDict)
                    {
                        list.Add(item + "/" + key);
                    }
                }
            }
            return list;
        }
    }
}