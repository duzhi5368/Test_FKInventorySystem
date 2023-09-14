using System.Collections.Generic;
using System.IO;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 游戏模块设置配置文件
    public class GameBootConfig
    {
        [ShowGUIName("后台运行模式")]
        public bool runInBackground = true;

        [NoShowInEditor]
        public Dictionary<string, ClassValue> allAppModuleSetting = new Dictionary<string, ClassValue>();

        public long buildTime;                                  // 打包时间,取值DateTime.Now.Ticks
        public const string ConfigFileName = "GameBootConfig";
        private const string SavePathDir = "Assets/GameCofig/Resources/";

        public static GameBootConfig LoadConfig()
        {
            string jsonData = null;
            if (Application.isEditor)
            {
                jsonData = ResourcesLoadConfig();
            }
            else
            {
                string fileName = ConfigFileName;
                string persistentDataPath = Application.persistentDataPath + "/Configs/" + fileName + ".txt";
                jsonData = ResourcesLoadConfig();
                if (!File.Exists(persistentDataPath))
                {
                    if (string.IsNullOrEmpty(jsonData))
                    {
                        return null;
                    }
                    else
                    {
                        Debug.Log("GameBootConfig写入沙盒");
                        FileUtils.CreateTextFile(persistentDataPath, jsonData);
                    }
                }
                else
                {
                    // 比较包里的配置和沙盒路径的配置buildTime，当不一致时 以包里的覆盖沙盒的，否则使用沙盒路径的（便于保存修改）
                    GameBootConfig resConfig = JsonSerializer.FromJson<GameBootConfig>(jsonData);
                    string perJsonData = FileUtils.LoadTextFileByPath(persistentDataPath);
                    GameBootConfig perConfig = JsonSerializer.FromJson<GameBootConfig>(perJsonData);
                    if (perConfig == null || perConfig.buildTime != resConfig.buildTime)
                    {
                        Debug.Log("GameBootConfig覆盖写入：" + resConfig.buildTime);
                        FileUtils.CreateTextFile(persistentDataPath, jsonData);
                        return resConfig;
                    }
                    else
                    {
                        return perConfig;
                    }
                }
            }
            return JsonSerializer.FromJson<GameBootConfig>(jsonData);
        }

        private static string ResourcesLoadConfig()
        {
            TextAsset textAsset = Resources.Load<TextAsset>(ConfigFileName);
            if (textAsset != null)
            {
                return textAsset.text;
            }
            else
            {
                return null;
            }
        }
        
        public static void Save(GameBootConfig config)
        {
            string json = JsonSerializer.ToJson(config);
            FileUtils.CreateTextFile(SavePathDir + ConfigFileName + ".txt", json);
        }
    }
}