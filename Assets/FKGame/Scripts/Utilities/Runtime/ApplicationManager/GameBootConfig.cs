using System.Collections.Generic;
using System.IO;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ��Ϸģ�����������ļ�
    public class GameBootConfig
    {
        [ShowGUIName("��̨����ģʽ")]
        public bool runInBackground = true;

        [NoShowInEditor]
        public Dictionary<string, ClassValue> allAppModuleSetting = new Dictionary<string, ClassValue>();

        public long buildTime;                                  // ���ʱ��,ȡֵDateTime.Now.Ticks
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
                        Debug.Log("GameBootConfigд��ɳ��");
                        FileUtils.CreateTextFile(persistentDataPath, jsonData);
                    }
                }
                else
                {
                    // �Ƚϰ�������ú�ɳ��·��������buildTime������һ��ʱ �԰���ĸ���ɳ�еģ�����ʹ��ɳ��·���ģ����ڱ����޸ģ�
                    GameBootConfig resConfig = JsonSerializer.FromJson<GameBootConfig>(jsonData);
                    string perJsonData = FileUtils.LoadTextFileByPath(persistentDataPath);
                    GameBootConfig perConfig = JsonSerializer.FromJson<GameBootConfig>(perJsonData);
                    if (perConfig == null || perConfig.buildTime != resConfig.buildTime)
                    {
                        Debug.Log("GameBootConfig����д�룺" + resConfig.buildTime);
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